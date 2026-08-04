/**
 * Hex Core Defense — HTML port of the Unity2d-Game tower defense.
 * Mechanics mirrored from Assets/Resources/Scripts/*
 */

import { loadAllAssets, loadSettings, DRAW_SIZES, buildBalance } from "./assets-config.js";
import { initSettings } from "./settings.js";

/** Runtime balance — rebuilt from settings on Apply */
let BALANCE = buildBalance(loadSettings());

const TUTORIAL = [
  {
    text: "This is your baby core. If it dies, you lose. Turrets defend it automatically.",
    highlight: "core",
  },
  {
    text: "Click any empty blue tile to open the build menu. Start with a Turret — it shoots enemies for you.",
    highlight: "empty",
  },
  {
    text: "Miners make gold over time. More miners = more turrets. You already have one free miner.",
    highlight: "miner",
  },
  {
    text: "You have a calm setup timer before enemies arrive. Build 1–2 turrets, then expand if you need space. Good luck!",
    highlight: "none",
  },
];

// Pointy-top hex neighbor offsets (axial coords)
const NEIGHBOR_OFFSETS = [
  { q: +1, r: 0 },
  { q: +1, r: -1 },
  { q: 0, r: -1 },
  { q: -1, r: 0 },
  { q: -1, r: +1 },
  { q: 0, r: +1 },
];

function axialToPixel(q, r) {
  // Pointy-top axial → pixel (scaled to ~90px cell art)
  const size = 48;
  return {
    x: size * Math.sqrt(3) * (q + r / 2),
    y: size * (3 / 2) * r,
  };
}

function clamp(v, a, b) {
  return Math.max(a, Math.min(b, v));
}

function dist(a, b) {
  const dx = a.x - b.x;
  const dy = a.y - b.y;
  return Math.hypot(dx, dy);
}

function angleBetween(a, b) {
  let ang = (Math.atan2(b.y - a.y, b.x - a.x) * 180) / Math.PI;
  if (ang < 0) ang += 360;
  return ang;
}

function dirFromAngle(deg) {
  const r = (deg * Math.PI) / 180;
  return { x: Math.cos(r), y: Math.sin(r) };
}

function closest(from, list) {
  let best = null;
  let bestD = Infinity;
  for (const o of list) {
    if (!o || o.dead || o.status !== "live") continue;
    const d = dist(from, o);
    if (d < bestD) {
      bestD = d;
      best = o;
    }
  }
  return best;
}

function cellKey(q, r) {
  return `${q},${r}`;
}

// ---- Entity base ----
class Entity {
  constructor(game, x, y) {
    this.game = game;
    this.x = x;
    this.y = y;
    this.dead = false;
    this.status = "live"; // build | live | inactive
    this.level = 0;
    this.health = 1;
    this.maxHealth = 1;
    this.radius = 22;
    this.flash = 0;
    this.buildTimer = 0;
    this.buildDuration = 0;
    this.showHealth = true;
    this.z = 0;
  }

  hit(dmg) {
    if (this.status !== "live" || this.dead) return;
    this.health -= dmg;
    this.flash = 0.15;
    if (this.health <= 0) this.die();
  }

  die() {
    this.dead = true;
    this.status = "destroyed";
  }

  startBuild(duration) {
    this.status = "build";
    this.buildDuration = duration;
    this.buildTimer = duration;
    this.showHealth = false;
  }

  finishBuild() {
    this.level++;
    this.status = "live";
    this.showHealth = true;
    this.onUpgraded?.();
  }

  update(dt) {
    if (this.flash > 0) this.flash -= dt;
    if (this.status === "build") {
      this.buildTimer -= dt;
      if (this.buildTimer <= 0) this.finishBuild();
    }
  }

  drawHealth(ctx, cam) {
    if (!this.showHealth || this.status !== "live") return;
    const sx = this.x - cam.x;
    const sy = this.y - cam.y + this.radius + 8;
    const w = 36;
    const h = 5;
    const t = clamp(this.health / this.maxHealth, 0, 1);
    ctx.fillStyle = "rgba(0,0,0,0.55)";
    ctx.fillRect(sx - w / 2, sy, w, h);
    ctx.fillStyle = t > 0.4 ? "#4ade80" : t > 0.2 ? "#fbbf24" : "#f87171";
    ctx.fillRect(sx - w / 2, sy, w * t, h);
  }
}

class Cell extends Entity {
  constructor(game, q, r) {
    const p = axialToPixel(q, r);
    super(game, p.x, p.y);
    this.q = q;
    this.r = r;
    this.type = "empty";
    this.building = null;
    this.selected = false;
    this.showHealth = false;
    this.radius = 40;
    this.z = -10;
  }

  canExpand() {
    return !this.building && this.status === "live" && this.game.minerals >= BALANCE.player.cell.expendCostPerLevel[0];
  }

  expand() {
    if (!this.canExpand()) return false;
    this.game.minerals -= BALANCE.player.cell.expendCostPerLevel[0];
    for (const off of NEIGHBOR_OFFSETS) {
      const nq = this.q + off.q;
      const nr = this.r + off.r;
      if (!this.game.cellMap.has(cellKey(nq, nr))) {
        this.game.addCell(nq, nr);
      }
    }
    this.status = "inactive";
    return true;
  }

  draw(ctx, cam, assets) {
    const sx = this.x - cam.x;
    const sy = this.y - cam.y;
    let img = assets.cell;
    if (this.status === "inactive") img = assets.cellInactive;
    if (this.selected) img = assets.cellSelected;
    if (!img) {
      ctx.fillStyle = this.selected ? "#f66" : this.status === "inactive" ? "#555" : "#6af";
      this.drawHex(ctx, sx, sy, 40);
      return;
    }
    const s = (DRAW_SIZES.cell || 100) * (this.game.scale("cell") || 1);
    ctx.save();
    ctx.globalAlpha = this.status === "inactive" ? 0.55 : 0.95;
    ctx.drawImage(img, sx - s / 2, sy - s / 2, s, s);
    ctx.restore();

    if (this.status === "build" || (this.building && this.building.status === "build")) {
      // building progress drawn by building
    }
  }

  drawHex(ctx, x, y, size) {
    ctx.beginPath();
    for (let i = 0; i < 6; i++) {
      const a = (Math.PI / 180) * (60 * i - 30);
      const px = x + size * Math.cos(a);
      const py = y + size * Math.sin(a);
      if (i === 0) ctx.moveTo(px, py);
      else ctx.lineTo(px, py);
    }
    ctx.closePath();
    ctx.fill();
  }
}

class Building extends Entity {
  constructor(game, cell, kind) {
    super(game, cell.x, cell.y);
    this.cell = cell;
    this.kind = kind;
    this.level = 0;
    cell.building = this;
    cell.type = kind;
  }

  die() {
    super.die();
    if (this.cell) {
      this.cell.building = null;
      this.cell.type = "empty";
    }
  }

  refund() {
    const cfg = this.cfg;
    const refund = cfg.refundPerLevel[Math.max(0, this.level - 1)] ?? cfg.refundPerLevel[0];
    this.game.minerals += refund;
    this.die();
  }

  canUpgrade() {
    const cfg = this.cfg;
    if (this.level >= cfg.buildCostPerLevel.length) return false;
    if (this.status !== "live") return false;
    return this.game.minerals >= cfg.buildCostPerLevel[this.level];
  }

  upgrade() {
    if (!this.canUpgrade()) return false;
    const cost = this.cfg.buildCostPerLevel[this.level];
    this.game.minerals -= cost;
    const t = this.cfg.upgradeTimePerLevel[this.level] ?? 1;
    this.startBuild(t);
    return true;
  }

  drawBuildOverlay(ctx, cam) {
    if (this.status !== "build") return;
    const sx = this.x - cam.x;
    const sy = this.y - cam.y;
    const p = 1 - this.buildTimer / this.buildDuration;
    ctx.save();
    ctx.strokeStyle = "rgba(94,234,212,0.9)";
    ctx.lineWidth = 3;
    ctx.beginPath();
    ctx.arc(sx, sy, 28, -Math.PI / 2, -Math.PI / 2 + Math.PI * 2 * p);
    ctx.stroke();
    ctx.fillStyle = "rgba(0,0,0,0.35)";
    ctx.beginPath();
    ctx.arc(sx, sy, 24, 0, Math.PI * 2);
    ctx.fill();
    ctx.fillStyle = "#5eead4";
    ctx.font = "bold 12px sans-serif";
    ctx.textAlign = "center";
    ctx.fillText(Math.ceil(this.buildTimer) + "s", sx, sy + 4);
    ctx.restore();
  }
}

class AutoCanon extends Building {
  constructor(game, cell) {
    super(game, cell, "autocanon");
    this.cfg = BALANCE.player.autoCanon;
    this.target = null;
    this.shootCd = 0;
    this.angle = 0;
    this.radius = 26;
    this.startBuild(this.cfg.upgradeTimePerLevel[0]);
  }

  onUpgraded() {
    const hp = this.cfg.healthPerLevel[this.level - 1] ?? 80;
    this.health = hp;
    this.maxHealth = hp;
    this.shootCd = this.cfg.shootSpeedPerLevel[this.level - 1] ?? 1;
  }

  update(dt) {
    super.update(dt);
    if (this.status !== "live") return;
    if (!this.target || this.target.dead) {
      this.target = closest(this, this.game.enemies);
    }
    if (this.target) {
      this.angle = angleBetween(this, this.target);
      this.shootCd -= dt;
      if (this.shootCd <= 0) {
        this.shoot();
        this.shootCd = this.cfg.shootSpeedPerLevel[this.level - 1] ?? 1;
      }
    }
  }

  shoot() {
    if (!this.target) return;
    const dmg = this.cfg.damagePerLevel[this.level - 1] ?? 10;
    this.game.spawnBullet({
      x: this.x,
      y: this.y,
      angle: this.angle,
      speed: this.cfg.bulletSpeed,
      damage: dmg,
      team: "player",
      hits: ["kamikazi", "destroyer", "orc", "goblin"],
      life: 1.8,
      color: "#fde68a",
    });
  }

  draw(ctx, cam, assets) {
    const sx = this.x - cam.x;
    const sy = this.y - cam.y;
    ctx.save();
    if (this.flash > 0) ctx.filter = "brightness(2) sepia(1) hue-rotate(-50deg)";
    const sc = this.game.scale("turret");
    const body = (DRAW_SIZES.turret || 56) * sc;
    const img = assets.cannon || assets.playerIcon;
    if (img) ctx.drawImage(img, sx - body / 2, sy - body / 2, body, body);
    else {
      ctx.fillStyle = "#94a3b8";
      ctx.beginPath();
      ctx.arc(sx, sy, 20 * sc, 0, Math.PI * 2);
      ctx.fill();
    }
    ctx.translate(sx, sy);
    ctx.rotate((this.angle * Math.PI) / 180);
    if (assets.cannonBarrel) {
      const bw = (DRAW_SIZES.barrelW || 34) * sc;
      const bh = (DRAW_SIZES.barrelH || 20) * sc;
      ctx.drawImage(assets.cannonBarrel, 0, -bh / 2, bw, bh);
    } else {
      ctx.fillStyle = "#cbd5e1";
      ctx.fillRect(0, -4 * sc, 28 * sc, 8 * sc);
    }
    ctx.restore();
    this.drawBuildOverlay(ctx, cam);
    this.drawHealth(ctx, cam);
    if (this.status === "live" && !this.game.hideLabels) {
      ctx.fillStyle = "rgba(15,23,42,0.75)";
      ctx.font = "bold 11px Segoe UI, sans-serif";
      ctx.textAlign = "center";
      const label = `Turret Lv${this.level}`;
      const tw = ctx.measureText(label).width + 10;
      ctx.fillRect(sx - tw / 2, sy + 30, tw, 16);
      ctx.fillStyle = "#fde68a";
      ctx.fillText(label, sx, sy + 42);
    }
  }

  die() {
    super.die();
    this.game.autoCanons = this.game.autoCanons.filter((b) => b !== this);
  }
}

class MineralMiner extends Building {
  constructor(game, cell) {
    super(game, cell, "miner");
    this.cfg = BALANCE.player.mineralMiner;
    this.tick = 0;
    this.radius = 28;
    this.startBuild(this.cfg.upgradeTimePerLevel[0]);
  }

  onUpgraded() {
    const hp = this.cfg.healthPerLevel[this.level - 1] ?? 40;
    this.health = hp;
    this.maxHealth = hp;
    this.tick = this.cfg.timeToMineralPerLevel[this.level - 1] ?? 2;
  }

  update(dt) {
    super.update(dt);
    if (this.status !== "live") return;
    this.tick -= dt;
    if (this.tick <= 0) {
      this.game.minerals += 1;
      this.game.floatText(this.x, this.y - 30, "+1", "#fbbf24");
      this.tick = this.cfg.timeToMineralPerLevel[this.level - 1] ?? 2;
    }
  }

  draw(ctx, cam, assets) {
    const sx = this.x - cam.x;
    const sy = this.y - cam.y;
    ctx.save();
    if (this.flash > 0) ctx.filter = "brightness(2)";
    const sc = this.game.scale("miner");
    const w = (DRAW_SIZES.miner || 72) * sc;
    const h = (DRAW_SIZES.minerH || 64) * sc;
    const img = assets.factory || assets.resource;
    if (img) ctx.drawImage(img, sx - w / 2, sy - h / 2, w, h);
    else {
      ctx.fillStyle = "#f59e0b";
      ctx.fillRect(sx - 22, sy - 18, 44, 36);
    }
    ctx.restore();
    this.drawBuildOverlay(ctx, cam);
    this.drawHealth(ctx, cam);
    if (this.status === "live" && !this.game.hideLabels) {
      ctx.fillStyle = "rgba(15,23,42,0.75)";
      ctx.font = "bold 11px Segoe UI, sans-serif";
      ctx.textAlign = "center";
      const label = `Miner +gold`;
      const tw = ctx.measureText(label).width + 10;
      ctx.fillRect(sx - tw / 2, sy + 32, tw, 16);
      ctx.fillStyle = "#fbbf24";
      ctx.fillText(label, sx, sy + 44);
    }
  }

  die() {
    super.die();
    this.game.mineralMiners = this.game.mineralMiners.filter((b) => b !== this);
  }
}

class PlayerCore extends Building {
  constructor(game, cell) {
    super(game, cell, "core");
    this.cfg = BALANCE.player.core;
    this.level = 1;
    this.health = this.cfg.healthPerLevel[0];
    this.maxHealth = this.health;
    this.status = "live";
    this.angle = 270;
    this.fireCd = 0;
    this.radius = 34;
    this.showHealth = true;
    this.frame = 0;
  }

  update(dt) {
    super.update(dt);
    if (this.status !== "live") return;
    this.fireCd -= dt;
    // Fully automatic — no player aiming required
    const t = closest(this, this.game.enemies);
    if (t && dist(this, t) < (this.cfg.autoRange || 420)) {
      this.angle = angleBetween(this, t);
      if (this.fireCd <= 0) this.fire(this.angle);
    } else if (this.game.worldMouse) {
      this.angle = angleBetween(this, this.game.worldMouse);
    }
    this.frame += dt * 4;
  }

  fire(angle) {
    this.fireCd = this.cfg.fireRate;
    const dmg = this.cfg.damagePerLevel[this.level - 1] ?? 5;
    this.game.spawnBullet({
      x: this.x,
      y: this.y,
      angle,
      speed: this.cfg.bulletSpeed,
      damage: dmg,
      team: "player",
      hits: ["kamikazi", "destroyer", "orc", "goblin"],
      life: 1.5,
      color: "#7dd3fc",
      scale: 1.4,
    });
  }

  draw(ctx, cam, assets) {
    const sx = this.x - cam.x;
    const sy = this.y - cam.y;
    ctx.save();
    if (this.flash > 0) ctx.filter = "brightness(2.2)";

    // soft glow
    const g = ctx.createRadialGradient(sx, sy, 10, sx, sy, 50);
    g.addColorStop(0, "rgba(94,234,212,0.35)");
    g.addColorStop(1, "rgba(94,234,212,0)");
    ctx.fillStyle = g;
    ctx.beginPath();
    ctx.arc(sx, sy, 50, 0, Math.PI * 2);
    ctx.fill();

    // pick baby frame by angle octant
    const oct = Math.floor(((this.angle + 22.5) % 360) / 45);
    const keys = ["baby2", "baby1", "baby0", "baby7", "baby6", "baby5", "baby4", "baby3"];
    const sc = this.game.scale("core");
    const cw = (DRAW_SIZES.core || 72) * sc;
    const ch = (DRAW_SIZES.coreH || 90) * sc;
    const img = assets[keys[oct]] || assets.baby0 || assets.playerIcon;
    if (img) {
      const bob = Math.sin(this.frame) * 2;
      ctx.drawImage(img, sx - cw / 2, sy - ch * 0.55 + bob, cw, ch);
    } else {
      ctx.fillStyle = "#fde68a";
      ctx.beginPath();
      ctx.arc(sx, sy, 28 * sc, 0, Math.PI * 2);
      ctx.fill();
    }

    ctx.translate(sx, sy);
    ctx.rotate((this.angle * Math.PI) / 180);
    if (assets.cannonBarrel) {
      ctx.globalAlpha = 0.85;
      ctx.drawImage(assets.cannonBarrel, 8 * sc, -12 * sc, 40 * sc, 24 * sc);
    }
    ctx.restore();
    this.drawHealth(ctx, cam);
  }

  die() {
    super.die();
    this.game.core = null;
    this.game.gameOver(false);
  }
}

class Enemy extends Entity {
  constructor(game, x, y, kind, level = 0) {
    super(game, x, y);
    this.kind = kind;
    this.level = level;
    this.target = null;
    this.attackCd = 0;
    this.anim = 0;
    this.angle = 0;
    this.flip = false;
  }

  onKillBonus() {
    const bonus = this.killBonus?.[this.level] ?? 1;
    this.game.minerals += bonus;
    this.game.score += bonus * 10 + 5;
    this.game.floatText(this.x, this.y - 20, `+${bonus}`, "#86efac");
  }

  die() {
    if (!this.dead) this.onKillBonus();
    super.die();
  }

  acquireTarget(list) {
    if (this.target && !this.target.dead && this.target.status === "live") return;
    this.target = closest(this, list);
  }

  moveToward(target, speed, dt) {
    if (!target) return;
    const d = dist(this, target);
    if (d < 1) return d;
    const vx = ((target.x - this.x) / d) * speed * dt;
    const vy = ((target.y - this.y) / d) * speed * dt;
    this.x += vx;
    this.y += vy;
    this.flip = target.x < this.x;
    this.angle = angleBetween(this, target);
    return d;
  }

  drawShadow(ctx, sx, sy) {
    ctx.fillStyle = "rgba(0,0,0,0.25)";
    ctx.beginPath();
    ctx.ellipse(sx, sy + 18, 16, 6, 0, 0, Math.PI * 2);
    ctx.fill();
  }
}

class Kamikazi extends Enemy {
  constructor(game, x, y, level = 0) {
    super(game, x, y, "kamikazi", level);
    const c = BALANCE.enemy.kamikazi;
    this.health = c.healthPerLevel[level] ?? 40;
    this.maxHealth = this.health;
    this.speed = c.speedPerLevel[level] ?? 55;
    this.damage = c.damagePerLevel[level] ?? 10;
    this.killBonus = c.killBonus;
    this.radius = 20;
  }

  update(dt) {
    super.update(dt);
    this.anim += dt * 8;
    const targets = [...this.game.mineralMiners];
    if (this.game.core) targets.push(this.game.core);
    this.acquireTarget(targets);
    if (this.target) {
      const d = this.moveToward(this.target, this.speed, dt);
      if (d < this.radius + this.target.radius) {
        this.target.hit(this.damage);
        this.game.floatText(this.target.x, this.target.y - 40, `-${this.damage}`, "#f87171");
        this.dead = true; // suicide — no kill bonus for ramming? give half
        this.game.score += 2;
      }
    }
  }

  draw(ctx, cam, assets) {
    const sx = this.x - cam.x;
    const sy = this.y - cam.y;
    this.drawShadow(ctx, sx, sy);
    // row by direction buckets, col by anim
    const row = Math.floor(((this.angle + 45) % 360) / 90) % 4;
    const col = Math.floor(this.anim) % 4;
    const sc = this.game.scale("kamikazi");
    const w = (DRAW_SIZES.kamikaziW || 64) * sc;
    const h = (DRAW_SIZES.kamikaziH || 40) * sc;
    const img = assets[`kami_${row}_${col}`] || assets.fighter;
    ctx.save();
    if (this.flash > 0) ctx.filter = "brightness(3)";
    if (img) ctx.drawImage(img, sx - w / 2, sy - h / 2, w, h);
    else {
      ctx.fillStyle = "#c084fc";
      ctx.beginPath();
      ctx.arc(sx, sy, 16 * sc, 0, Math.PI * 2);
      ctx.fill();
    }
    ctx.restore();
    this.drawHealth(ctx, cam);
  }
}

class Destroyer extends Enemy {
  constructor(game, x, y, level = 0) {
    super(game, x, y, "destroyer", level);
    const c = BALANCE.enemy.destroyer;
    this.health = c.healthPerLevel[level] ?? 120;
    this.maxHealth = this.health;
    this.speed = c.speedPerLevel[level] ?? 35;
    this.damage = c.damagePerLevel[level] ?? 8;
    this.range = c.rangePerLevel[level] ?? 160;
    this.shootSpeed = c.shootSpeedPerLevel[level] ?? 1.2;
    this.killBonus = c.killBonus;
    this.radius = 24;
    this.attackCd = 1;
  }

  update(dt) {
    super.update(dt);
    this.anim += dt * 6;
    this.acquireTarget(this.game.autoCanons);
    if (!this.target && this.game.core) this.target = this.game.core;
    if (!this.target) return;
    const d = dist(this, this.target);
    this.angle = angleBetween(this, this.target);
    if (d > this.range) {
      this.moveToward(this.target, this.speed, dt);
    } else {
      this.attackCd -= dt;
      if (this.attackCd <= 0) {
        this.attackCd = this.shootSpeed;
        this.game.spawnBullet({
          x: this.x,
          y: this.y,
          angle: this.angle,
          speed: 260,
          damage: this.damage,
          team: "enemy",
          hits: ["autocanon", "core", "miner"],
          life: 2.2,
          color: "#f472b6",
        });
      }
    }
  }

  draw(ctx, cam, assets) {
    const sx = this.x - cam.x;
    const sy = this.y - cam.y;
    this.drawShadow(ctx, sx, sy);
    const row = Math.floor(((this.angle + 45) % 360) / 90) % 4;
    const col = Math.floor(this.anim) % 4;
    const sc = this.game.scale("destroyer");
    const w = (DRAW_SIZES.destroyerW || 72) * sc;
    const h = (DRAW_SIZES.destroyerH || 48) * sc;
    const img = assets[`dest_${row}_${col}`] || assets.bomber;
    ctx.save();
    if (this.flash > 0) ctx.filter = "brightness(3)";
    if (img) ctx.drawImage(img, sx - w / 2, sy - h / 2, w, h);
    else {
      ctx.fillStyle = "#fb7185";
      ctx.fillRect(sx - 18, sy - 14, 36, 28);
    }
    ctx.restore();
    this.drawHealth(ctx, cam);
  }
}

class Orc extends Enemy {
  constructor(game, x, y, level = 0) {
    super(game, x, y, "orc", level);
    const c = BALANCE.enemy.orc;
    this.health = c.healthPerLevel[level] ?? 90;
    this.maxHealth = this.health;
    this.speed = c.speedPerLevel[level] ?? 40;
    this.damage = c.damagePerLevel[level] ?? 15;
    this.attackSpeed = c.attackSpeedPerLevel[level] ?? 1.2;
    this.meleeRange = c.meleeRange;
    this.killBonus = c.killBonus;
    this.radius = 26;
    this.attackCd = 0.5;
  }

  update(dt) {
    super.update(dt);
    this.anim += dt;
    const targets = [...this.game.mineralMiners, ...this.game.autoCanons];
    if (this.game.core) targets.push(this.game.core);
    this.acquireTarget(targets);
    if (!this.target) return;
    const d = this.moveToward(this.target, this.speed, dt);
    if (d <= this.meleeRange + this.target.radius) {
      this.attackCd -= dt;
      if (this.attackCd <= 0) {
        this.attackCd = this.attackSpeed;
        this.target.hit(this.damage);
        this.game.floatText(this.target.x, this.target.y - 30, `-${this.damage}`, "#f87171");
        this.swing = 0.2;
      }
    }
    if (this.swing > 0) this.swing -= dt;
  }

  draw(ctx, cam, assets) {
    const sx = this.x - cam.x;
    const sy = this.y - cam.y;
    this.drawShadow(ctx, sx, sy);
    ctx.save();
    if (this.flash > 0) ctx.filter = "brightness(3)";
    if (this.flip) {
      ctx.translate(sx, sy);
      ctx.scale(-1, 1);
      ctx.translate(-sx, -sy);
    }
    const sc = this.game.scale("orc");
    const w = (DRAW_SIZES.orcW || 60) * sc;
    const h = (DRAW_SIZES.orcH || 70) * sc;
    const bob = Math.sin(this.anim * 8) * 2;
    const img = assets.orc || assets.fighter;
    if (img) ctx.drawImage(img, sx - w / 2, sy - h * 0.55 + bob, w, h);
    else {
      ctx.fillStyle = "#4ade80";
      ctx.beginPath();
      ctx.arc(sx, sy, 20 * sc, 0, Math.PI * 2);
      ctx.fill();
    }
    if (this.swing > 0) {
      ctx.strokeStyle = "rgba(255,255,255,0.7)";
      ctx.lineWidth = 3;
      ctx.beginPath();
      ctx.arc(sx + 10, sy, 24 * sc, -0.8, 0.8);
      ctx.stroke();
    }
    ctx.restore();
    this.drawHealth(ctx, cam);
  }
}

class Goblin extends Enemy {
  constructor(game, x, y, level = 0) {
    super(game, x, y, "goblin", level);
    const c = BALANCE.enemy.goblin;
    this.health = c.healthPerLevel[level] ?? 55;
    this.maxHealth = this.health;
    this.speed = c.speedPerLevel[level] ?? 48;
    this.damage = c.damagePerLevel[level] ?? 6;
    this.range = c.rangePerLevel[level] ?? 150;
    this.attackSpeed = c.attackSpeedPerLevel[level] ?? 1.3;
    this.killBonus = c.killBonus;
    this.radius = 22;
    this.attackCd = 0.8;
  }

  update(dt) {
    super.update(dt);
    this.anim += dt;
    const targets = [...this.game.mineralMiners, ...this.game.autoCanons];
    if (this.game.core) targets.push(this.game.core);
    this.acquireTarget(targets);
    if (!this.target) return;
    const d = dist(this, this.target);
    this.angle = angleBetween(this, this.target);
    this.flip = this.target.x < this.x;
    if (d > this.range) {
      this.moveToward(this.target, this.speed, dt);
    } else {
      this.attackCd -= dt;
      if (this.attackCd <= 0) {
        this.attackCd = this.attackSpeed;
        this.game.spawnBullet({
          x: this.x,
          y: this.y,
          angle: this.angle,
          speed: 300,
          damage: this.damage,
          team: "enemy",
          hits: ["autocanon", "core", "miner"],
          life: 2,
          color: "#86efac",
          arrow: true,
        });
      }
    }
  }

  draw(ctx, cam, assets) {
    const sx = this.x - cam.x;
    const sy = this.y - cam.y;
    this.drawShadow(ctx, sx, sy);
    ctx.save();
    if (this.flash > 0) ctx.filter = "brightness(3)";
    if (this.flip) {
      ctx.translate(sx, sy);
      ctx.scale(-1, 1);
      ctx.translate(-sx, -sy);
    }
    const sc = this.game.scale("goblin");
    const w = (DRAW_SIZES.goblinW || 56) * sc;
    const h = (DRAW_SIZES.goblinH || 64) * sc;
    const bob = Math.sin(this.anim * 10) * 2;
    const img = assets.goblin || assets.fighter;
    if (img) ctx.drawImage(img, sx - w / 2, sy - h * 0.55 + bob, w, h);
    else {
      ctx.fillStyle = "#22c55e";
      ctx.beginPath();
      ctx.arc(sx, sy, 16 * sc, 0, Math.PI * 2);
      ctx.fill();
    }
    ctx.restore();
    this.drawHealth(ctx, cam);
  }
}

class Game {
  constructor(canvas, assets) {
    this.canvas = canvas;
    this.ctx = canvas.getContext("2d");
    this.assets = assets;
    this.applyAssetSettings(assets);
    this.cells = [];
    this.cellMap = new Map();
    this.buildings = [];
    this.autoCanons = [];
    this.mineralMiners = [];
    this.enemies = [];
    this.bullets = [];
    this.floats = [];
    this.particles = [];
    this.core = null;
    this.minerals = BALANCE.startMinerals;
    this.score = 0;
    this.wave = 0;
    this.time = 0;
    this.spawnTimer = BALANCE.gracePeriod;
    this.graceLeft = BALANCE.gracePeriod;
    this.combatStarted = false;
    this.running = false;
    this.paused = false;
    this.selectedCell = null;
    this.hoverCell = null;
    this.tutorialStep = 0;
    this.tutorialDone = false;
    this.highlightMode = "none";
    this.pulse = 0;
    this.cam = { x: 0, y: 0 };
    this.worldMouse = { x: 0, y: 0 };
    this.screenMouse = { x: 0, y: 0 };
    this.dragCam = null;
    this._mineralsEl = document.getElementById("minerals");
    this._scoreEl = document.getElementById("score");
    this._hpEl = document.getElementById("core-hp");
    this._waveEl = document.getElementById("wave");
    this._goalText = document.getElementById("goal-text");
    this._waveTimer = document.getElementById("wave-timer");
    this._actionPanel = document.getElementById("action-panel");
    this._apTitle = document.getElementById("ap-title");
    this._apSub = document.getElementById("ap-sub");
    this._apBtns = document.getElementById("ap-btns");
    this._coach = document.getElementById("coach");
    this._coachText = document.getElementById("coach-text");
    this._coachStep = document.getElementById("coach-step");
    this._toast = document.getElementById("toast");
    this.toastTimer = 0;
    this.resize();
    window.addEventListener("resize", () => this.resize());
  }

  applyAssetSettings(assets) {
    this.assets = assets;
    const s = assets.__settings || loadSettings();
    this.assetSettings = s;
    this.hideLabels = !!s.hideLabels;
  }

  scale(key) {
    return this.assetSettings?.scales?.[key] ?? 1;
  }

  resize() {
    const dpr = Math.min(window.devicePixelRatio || 1, 2);
    this.canvas.width = Math.floor(window.innerWidth * dpr);
    this.canvas.height = Math.floor(window.innerHeight * dpr);
    this.canvas.style.width = window.innerWidth + "px";
    this.canvas.style.height = window.innerHeight + "px";
    this.ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
    this.w = window.innerWidth;
    this.h = window.innerHeight;
  }

  reset() {
    this.cells = [];
    this.cellMap.clear();
    this.buildings = [];
    this.autoCanons = [];
    this.mineralMiners = [];
    this.enemies = [];
    this.bullets = [];
    this.floats = [];
    this.particles = [];
    this.minerals = BALANCE.startMinerals;
    this.score = 0;
    this.wave = 0;
    this.time = 0;
    this.spawnTimer = BALANCE.gracePeriod;
    this.graceLeft = BALANCE.gracePeriod;
    this.combatStarted = false;
    this.kills = 0;
    this.running = true;
    this.paused = false;
    this.selectedCell = null;
    this.hoverCell = null;
    this.tutorialStep = 0;
    this.tutorialDone = false;
    this.highlightMode = "core";
    this.hideActionPanel();

    this.addCell(0, 0);
    for (const off of NEIGHBOR_OFFSETS) this.addCell(off.q, off.r);

    const center = this.cellMap.get("0,0");
    this.core = new PlayerCore(this, center);
    this.buildings.push(this.core);

    const c1 = this.cellMap.get(cellKey(1, 0));
    if (c1) {
      const m = new MineralMiner(this, c1);
      m.buildTimer = 0.01;
      this.mineralMiners.push(m);
      this.buildings.push(m);
    }

    // free starter turret so player sees defense immediately
    const c2 = this.cellMap.get(cellKey(-1, 0));
    if (c2) {
      const t = new AutoCanon(this, c2);
      t.buildTimer = 0.01;
      this.autoCanons.push(t);
      this.buildings.push(t);
    }

    this.centerCamera();
    this.updateHud();
    this.showTutorial(0);
    this.setGoal("Setup time — click blue tiles to build. Enemies arrive soon.");
  }

  centerCamera() {
    this.cam.x = -this.w / 2;
    this.cam.y = -this.h / 2 + 20;
  }

  addCell(q, r) {
    const k = cellKey(q, r);
    if (this.cellMap.has(k)) return this.cellMap.get(k);
    const cell = new Cell(this, q, r);
    this.cells.push(cell);
    this.cellMap.set(k, cell);
    return cell;
  }

  clearSelection() {
    for (const c of this.cells) c.selected = false;
    this.selectedCell = null;
  }

  cellAtWorld(x, y) {
    let best = null;
    let bestD = 44;
    for (const c of this.cells) {
      const d = dist({ x, y }, c);
      if (d < bestD) {
        bestD = d;
        best = c;
      }
    }
    return best;
  }

  selectCell(cell) {
    this.clearSelection();
    if (!cell || cell.status === "inactive") {
      this.hideActionPanel();
      return;
    }
    cell.selected = true;
    this.selectedCell = cell;
    this.showActionPanel(cell);
  }

  hideActionPanel() {
    this._actionPanel.classList.add("hidden");
    this.clearSelection();
  }

  showActionPanel(cell) {
    const panel = this._actionPanel;
    const sx = cell.x - this.cam.x;
    const sy = cell.y - this.cam.y;
    let left = clamp(sx, 150, this.w - 150);
    let top = clamp(sy - 20, 160, this.h - 40);
    panel.style.left = left + "px";
    panel.style.top = top + "px";
    panel.classList.remove("hidden");

    const btns = this._apBtns;
    btns.innerHTML = "";

    const addBtn = ({ icon, name, desc, price, disabled, primary, danger, onClick }) => {
      const b = document.createElement("button");
      b.className = "ap-btn" + (primary ? " primary" : "") + (danger ? " danger" : "");
      b.disabled = !!disabled;
      b.innerHTML = `
        <span class="icon">${icon}</span>
        <span class="meta"><span class="name">${name}</span><span class="desc">${desc}</span></span>
        <span class="price">${price ?? ""}</span>`;
      b.addEventListener("click", (e) => {
        e.stopPropagation();
        onClick();
      });
      btns.appendChild(b);
    };

    if (!cell.building) {
      this._apTitle.textContent = "Empty tile";
      this._apSub.textContent = "Pick one action for this tile.";
      const tCost = BALANCE.player.autoCanon.buildCostPerLevel[0];
      const mCost = BALANCE.player.mineralMiner.buildCostPerLevel[0];
      const eCost = BALANCE.player.cell.expendCostPerLevel[0];

      addBtn({
        icon: "🔫",
        name: "Build Turret",
        desc: "Auto-shoots nearby enemies",
        price: `${tCost} gold`,
        primary: true,
        disabled: this.minerals < tCost,
        onClick: () => this.buildOn(cell, "autocanon"),
      });
      addBtn({
        icon: "🏭",
        name: "Build Miner",
        desc: "Produces gold over time",
        price: `${mCost} gold`,
        disabled: this.minerals < mCost,
        onClick: () => this.buildOn(cell, "miner"),
      });
      addBtn({
        icon: "📐",
        name: "Expand base",
        desc: "Grow new tiles around this one",
        price: `${eCost} gold`,
        disabled: this.minerals < eCost,
        onClick: () => this.expandCell(cell),
      });
    } else if (cell.building.kind === "core") {
      this._apTitle.textContent = "Baby Core";
      this._apSub.textContent = "Keep this alive. It shoots automatically.";
      addBtn({
        icon: "ℹ️",
        name: "Your main base",
        desc: `HP ${Math.ceil(cell.building.health)} / ${cell.building.maxHealth}`,
        price: "",
        disabled: true,
        onClick: () => {},
      });
    } else {
      const bld = cell.building;
      const isTurret = bld.kind === "autocanon";
      this._apTitle.textContent = isTurret ? `Turret · Level ${bld.level}` : `Miner · Level ${bld.level}`;
      this._apSub.textContent = isTurret
        ? "Defends your base automatically."
        : "Generates gold. Upgrade for faster income.";

      if (bld.status === "live" && bld.canUpgrade()) {
        const cost = bld.cfg.buildCostPerLevel[bld.level];
        addBtn({
          icon: "⬆️",
          name: "Upgrade",
          desc: isTurret ? "More damage & fire rate" : "Faster gold production",
          price: `${cost} gold`,
          primary: true,
          disabled: this.minerals < cost,
          onClick: () => {
            if (bld.upgrade()) {
              this.toast("Upgrading…");
              this.floatText(cell.x, cell.y, "Upgrading…", "#fbbf24");
              this.hideActionPanel();
              this.updateHud();
            }
          },
        });
      } else if (bld.status === "build") {
        addBtn({
          icon: "⏳",
          name: "Building…",
          desc: "Please wait a moment",
          price: "",
          disabled: true,
          onClick: () => {},
        });
      } else if (bld.status === "live") {
        addBtn({
          icon: "⬆️",
          name: "Max level",
          desc: "This building is fully upgraded",
          price: "",
          disabled: true,
          onClick: () => {},
        });
      }

      const refund = bld.cfg.refundPerLevel[Math.max(0, bld.level - 1)] ?? 0;
      addBtn({
        icon: "💵",
        name: "Sell",
        desc: "Remove building and get gold back",
        price: `+${refund}`,
        danger: true,
        onClick: () => {
          bld.refund();
          this.buildings = this.buildings.filter((b) => !b.dead);
          this.floatText(cell.x, cell.y, "Sold", "#fbbf24");
          this.toast(`Sold for ${refund} gold`);
          this.hideActionPanel();
          this.updateHud();
        },
      });
    }
  }

  buildOn(cell, kind) {
    if (!cell || cell.building || cell.status !== "live") return;
    if (kind === "autocanon") {
      const cost = BALANCE.player.autoCanon.buildCostPerLevel[0];
      if (this.minerals < cost) return this.toast("Not enough gold");
      this.minerals -= cost;
      const b = new AutoCanon(this, cell);
      this.autoCanons.push(b);
      this.buildings.push(b);
      this.floatText(cell.x, cell.y - 20, "Turret!", "#5eead4");
      this.toast("Turret building — it will shoot on its own");
    } else {
      const cost = BALANCE.player.mineralMiner.buildCostPerLevel[0];
      if (this.minerals < cost) return this.toast("Not enough gold");
      this.minerals -= cost;
      const b = new MineralMiner(this, cell);
      this.mineralMiners.push(b);
      this.buildings.push(b);
      this.floatText(cell.x, cell.y - 20, "Miner!", "#fbbf24");
      this.toast("Miner building — more gold incoming");
    }
    this.hideActionPanel();
    this.updateHud();
    if (!this.tutorialDone && this.tutorialStep === 1) this.advanceTutorial();
  }

  expandCell(cell) {
    if (!cell.expand()) {
      this.toast("Can't expand here");
      return;
    }
    this.floatText(cell.x, cell.y, "Expanded!", "#5eead4");
    this.toast("New tiles unlocked");
    this.hideActionPanel();
    this.updateHud();
  }

  showTutorial(step) {
    this.tutorialStep = step;
    if (step >= TUTORIAL.length) {
      this.tutorialDone = true;
      this.highlightMode = "none";
      this._coach.classList.add("hidden");
      return;
    }
    const t = TUTORIAL[step];
    this.highlightMode = t.highlight;
    this._coach.classList.remove("hidden");
    this._coachStep.textContent = `${step + 1}/${TUTORIAL.length}`;
    this._coachText.textContent = t.text;
    document.getElementById("coach-next").textContent =
      step === TUTORIAL.length - 1 ? "Let's go!" : "Next";
  }

  advanceTutorial() {
    this.showTutorial(this.tutorialStep + 1);
  }

  setGoal(text) {
    this._goalText.textContent = text;
  }

  toast(msg) {
    this._toast.textContent = msg;
    this._toast.classList.remove("hidden");
    this.toastTimer = 2.4;
  }

  spawnBullet(opts) {
    this.bullets.push({ ...opts, age: 0, dead: false });
  }

  floatText(x, y, text, color) {
    this.floats.push({ x, y, text, color, age: 0, life: 1.1 });
  }

  spawnEnemy() {
    const angle = Math.random() * Math.PI * 2;
    const radius = 380 + Math.random() * 100 + this.wave * 8;
    const x = Math.cos(angle) * radius;
    const y = Math.sin(angle) * radius;
    const level = Math.min(2, Math.floor(Math.max(0, this.wave - 1) / 4));
    const roll = Math.random();
    let e;
    // early waves: only simple kamikazi
    if (this.wave <= 1 || roll < 0.5) e = new Kamikazi(this, x, y, level);
    else if (this.wave <= 3 || roll < 0.7) e = new Goblin(this, x, y, level);
    else if (roll < 0.88) e = new Orc(this, x, y, level);
    else e = new Destroyer(this, x, y, level);
    this.enemies.push(e);
  }

  update(dt) {
    if (!this.running || this.paused) return;
    this.time += dt;
    this.pulse += dt;
    if (this.toastTimer > 0) {
      this.toastTimer -= dt;
      if (this.toastTimer <= 0) this._toast.classList.add("hidden");
    }

    // Grace period then waves
    if (!this.combatStarted) {
      this.graceLeft -= dt;
      if (this.graceLeft <= 0) {
        this.combatStarted = true;
        this.wave = 1;
        this.spawnTimer = 0.5;
        this.setGoal("Wave 1 — enemies incoming! Turrets will fight for you.");
        this.toast("Wave 1 started!");
      }
    } else {
      const waveLen = BALANCE.waveSeconds || 32;
      const newWave = 1 + Math.floor((this.time - BALANCE.gracePeriod) / waveLen);
      if (newWave > this.wave) {
        this.wave = newWave;
        this.setGoal(`Wave ${this.wave} — keep building turrets and miners.`);
        this.toast(`Wave ${this.wave}!`);
      }
      this.spawnTimer -= dt;
      const base = BALANCE.spawnInterval ?? 6.5;
      const minI = BALANCE.spawnIntervalMin ?? 1.8;
      const spawnEvery = Math.max(minI, base - this.wave * 0.28);
      if (this.spawnTimer <= 0) {
        const count = BALANCE.spawnCount || 1;
        for (let i = 0; i < count; i++) this.spawnEnemy();
        if (Math.random() < (BALANCE.extraSpawnChance || 0)) this.spawnEnemy();
        this.spawnTimer = spawnEvery;
      }
    }

    for (const b of this.buildings) if (!b.dead) b.update(dt);
    for (const e of this.enemies) if (!e.dead) e.update(dt);

    for (const b of this.bullets) {
      if (b.dead) continue;
      b.age += dt;
      if (b.age > b.life) {
        b.dead = true;
        continue;
      }
      const d = dirFromAngle(b.angle);
      b.x += d.x * b.speed * dt;
      b.y += d.y * b.speed * dt;

      const targets =
        b.team === "player"
          ? this.enemies
          : [...this.autoCanons, ...this.mineralMiners, this.core].filter(Boolean);

      for (const t of targets) {
        if (!t || t.dead || t.status !== "live") continue;
        const kind = t.kind;
        if (b.hits && kind && !b.hits.includes(kind)) continue;
        if (dist(b, t) < t.radius + 8) {
          t.hit(b.damage);
          b.dead = true;
          this.particles.push({ x: b.x, y: b.y, age: 0, life: 0.25, color: b.color || "#fff" });
          break;
        }
      }
    }

    this.bullets = this.bullets.filter((b) => !b.dead);
    this.enemies = this.enemies.filter((e) => !e.dead);
    this.buildings = this.buildings.filter((b) => !b.dead);
    this.autoCanons = this.autoCanons.filter((b) => !b.dead);
    this.mineralMiners = this.mineralMiners.filter((b) => !b.dead);

    for (const f of this.floats) {
      f.age += dt;
      f.y -= 28 * dt;
    }
    this.floats = this.floats.filter((f) => f.age < f.life);
    for (const p of this.particles) p.age += dt;
    this.particles = this.particles.filter((p) => p.age < p.life);

    if (this.core) {
      const tx = this.core.x - this.w / 2;
      const ty = this.core.y - this.h / 2 + 10;
      this.cam.x += (tx - this.cam.x) * 0.04;
      this.cam.y += (ty - this.cam.y) * 0.04;
    }

    // keep action panel anchored
    if (this.selectedCell && !this._actionPanel.classList.contains("hidden")) {
      const sx = this.selectedCell.x - this.cam.x;
      const sy = this.selectedCell.y - this.cam.y;
      this._actionPanel.style.left = clamp(sx, 150, this.w - 150) + "px";
      this._actionPanel.style.top = clamp(sy - 20, 160, this.h - 40) + "px";
    }

    this.updateHud();
  }

  updateHud() {
    this._mineralsEl.textContent = String(this.minerals);
    this._scoreEl.textContent = String(this.score);
    this._waveEl.textContent = String(Math.max(1, this.wave) || "—");
    this._hpEl.textContent = this.core ? String(Math.max(0, Math.ceil(this.core.health))) : "0";

    if (!this.combatStarted) {
      const s = Math.max(0, Math.ceil(this.graceLeft));
      this._waveTimer.textContent = `Enemies in ${s}s`;
      this._waveEl.textContent = "—";
    } else {
      this._waveTimer.textContent = `${this.enemies.length} enemies`;
    }
  }

  drawBackground() {
    const ctx = this.ctx;
    const g = ctx.createRadialGradient(this.w * 0.5, this.h * 0.4, 40, this.w * 0.5, this.h * 0.5, this.w * 0.7);
    g.addColorStop(0, "#1a2748");
    g.addColorStop(1, "#0b1020");
    ctx.fillStyle = g;
    ctx.fillRect(0, 0, this.w, this.h);

    ctx.fillStyle = "rgba(255,255,255,0.35)";
    for (let i = 0; i < 60; i++) {
      const x = (i * 97 + this.time * 3) % this.w;
      const y = (i * 53) % this.h;
      ctx.fillRect(x, y, i % 5 === 0 ? 2 : 1, i % 5 === 0 ? 2 : 1);
    }
  }

  drawHighlights(ctx, cam) {
    if (this.highlightMode === "none" && !this.hoverCell) return;
    const pulse = 0.45 + Math.sin(this.pulse * 4) * 0.25;

    const ring = (ent, color) => {
      if (!ent) return;
      const sx = ent.x - cam.x;
      const sy = ent.y - cam.y;
      ctx.save();
      ctx.strokeStyle = color;
      ctx.globalAlpha = pulse;
      ctx.lineWidth = 3;
      ctx.beginPath();
      ctx.arc(sx, sy, 42, 0, Math.PI * 2);
      ctx.stroke();
      ctx.restore();
    };

    if (this.highlightMode === "core" && this.core) ring(this.core, "#5eead4");
    if (this.highlightMode === "miner") {
      for (const m of this.mineralMiners) ring(m, "#fbbf24");
    }
    if (this.highlightMode === "empty") {
      for (const c of this.cells) {
        if (!c.building && c.status === "live") ring(c, "#93c5fd");
      }
    }

    // hover affordance
    if (this.hoverCell && this.hoverCell.status === "live") {
      const sx = this.hoverCell.x - cam.x;
      const sy = this.hoverCell.y - cam.y;
      ctx.save();
      ctx.strokeStyle = "rgba(255,255,255,0.55)";
      ctx.lineWidth = 2;
      ctx.setLineDash([6, 4]);
      ctx.beginPath();
      ctx.arc(sx, sy, 40, 0, Math.PI * 2);
      ctx.stroke();
      ctx.restore();
    }
  }

  draw() {
    const ctx = this.ctx;
    const cam = this.cam;
    this.drawBackground();

    ctx.save();
    ctx.translate(-cam.x, -cam.y);
    const rg = ctx.createRadialGradient(0, 0, 80, 0, 0, 520);
    rg.addColorStop(0, "rgba(45, 212, 191, 0.06)");
    rg.addColorStop(1, "rgba(0,0,0,0)");
    ctx.fillStyle = rg;
    ctx.beginPath();
    ctx.arc(0, 0, 520, 0, Math.PI * 2);
    ctx.fill();
    ctx.restore();

    const drawables = [...this.cells, ...this.buildings, ...this.enemies];
    drawables.sort((a, b) => a.y + (a.z || 0) - (b.y + (b.z || 0)));
    for (const d of drawables) {
      if (!d.dead) d.draw(ctx, cam, this.assets);
    }

    this.drawHighlights(ctx, cam);

    for (const b of this.bullets) {
      const sx = b.x - cam.x;
      const sy = b.y - cam.y;
      ctx.save();
      ctx.translate(sx, sy);
      ctx.rotate((b.angle * Math.PI) / 180);
      if (b.arrow) {
        ctx.fillStyle = b.color;
        ctx.beginPath();
        ctx.moveTo(10, 0);
        ctx.lineTo(-8, 3);
        ctx.lineTo(-8, -3);
        ctx.closePath();
        ctx.fill();
      } else if (this.assets.bullet) {
        const s = (DRAW_SIZES.bullet || 10) * this.scale("bullet") * (b.scale || 1);
        ctx.drawImage(this.assets.bullet, -s / 2, -s / 2, s, s);
        ctx.fillStyle = b.color;
        ctx.globalAlpha = 0.5;
        ctx.beginPath();
        ctx.arc(0, 0, 4, 0, Math.PI * 2);
        ctx.fill();
      } else {
        ctx.fillStyle = b.color || "#fff";
        ctx.beginPath();
        ctx.arc(0, 0, 4, 0, Math.PI * 2);
        ctx.fill();
      }
      ctx.restore();
    }

    for (const p of this.particles) {
      const t = 1 - p.age / p.life;
      ctx.globalAlpha = t;
      ctx.fillStyle = p.color;
      ctx.beginPath();
      ctx.arc(p.x - cam.x, p.y - cam.y, 10 * t, 0, Math.PI * 2);
      ctx.fill();
      ctx.globalAlpha = 1;
    }

    for (const f of this.floats) {
      const t = 1 - f.age / f.life;
      ctx.globalAlpha = t;
      ctx.fillStyle = f.color;
      ctx.font = "bold 14px Segoe UI, sans-serif";
      ctx.textAlign = "center";
      ctx.fillText(f.text, f.x - cam.x, f.y - cam.y);
      ctx.globalAlpha = 1;
    }

    // clickable hint during setup
    if (this.running && !this.combatStarted && this.tutorialDone) {
      ctx.fillStyle = "rgba(148,163,184,0.85)";
      ctx.font = "13px Segoe UI, sans-serif";
      ctx.textAlign = "center";
      ctx.fillText("Click a blue tile to build", this.w / 2, this.h - 28);
    }

    if (this.paused) {
      ctx.fillStyle = "rgba(0,0,0,0.4)";
      ctx.fillRect(0, 0, this.w, this.h);
      ctx.fillStyle = "#e2e8f0";
      ctx.font = "bold 36px Segoe UI, sans-serif";
      ctx.textAlign = "center";
      ctx.fillText("Paused", this.w / 2, this.h / 2);
    }
  }

  gameOver(won) {
    this.running = false;
    this.hideActionPanel();
    this._coach.classList.add("hidden");
    const overlay = document.getElementById("overlay");
    const title = document.getElementById("overlay-title");
    const msg = document.getElementById("overlay-msg");
    const btn = document.getElementById("btn-start");
    overlay.classList.remove("hidden");
    title.textContent = won ? "Victory!" : "Core Destroyed";
    msg.textContent = `Score ${this.score} · Reached wave ${this.wave} · Gold left ${this.minerals}`;
    btn.textContent = "Play Again";
    const flow = document.querySelector(".flow");
    if (flow) flow.style.display = "none";
    const badge = document.querySelector(".panel-badge");
    if (badge) badge.textContent = won ? "NICE" : "TRY AGAIN";
    const tiny = document.querySelector(".tiny");
    if (tiny) tiny.textContent = "Tip: build more turrets before expanding";
  }

  screenToWorld(sx, sy) {
    return { x: sx + this.cam.x, y: sy + this.cam.y };
  }
}

// ---- bootstrap ----
(async function main() {
  const canvas = document.getElementById("game");
  const overlay = document.getElementById("overlay");

  const assets = await loadAllAssets(loadSettings());
  const game = new Game(canvas, assets);
  window.game = game;

  initSettings({
    getGame: () => game,
    onSettingsApplied: ({ assets, balance }) => {
      BALANCE = balance;
      game.applyAssetSettings(assets);
      // refresh live entity stats where safe
      if (game.core && game.core.status === "live") {
        const max = BALANCE.player.core.healthPerLevel[0];
        const ratio = game.core.health / (game.core.maxHealth || max);
        game.core.maxHealth = max;
        game.core.health = Math.max(1, Math.round(max * ratio));
        game.core.cfg = BALANCE.player.core;
      }
      if (game.running) {
        game.toast("Settings applied — new builds/enemies use new values");
      } else {
        game.toast?.("Settings saved");
      }
    },
  });

  document.getElementById("btn-start").addEventListener("click", () => {
    overlay.classList.add("hidden");
    const flow = document.querySelector(".flow");
    if (flow) flow.style.display = "";
    const badge = document.querySelector(".panel-badge");
    if (badge) badge.textContent = "EASY MODE";
    game.reset();
  });

  document.getElementById("btn-pause").addEventListener("click", () => {
    if (!game.running) return;
    game.paused = !game.paused;
  });

  document.getElementById("coach-next").addEventListener("click", () => {
    game.advanceTutorial();
  });

  document.getElementById("ap-close").addEventListener("click", () => {
    game.hideActionPanel();
  });

  function onPointer(e, type) {
    const rect = canvas.getBoundingClientRect();
    const clientX = e.clientX ?? e.changedTouches?.[0]?.clientX ?? e.touches?.[0]?.clientX;
    const clientY = e.clientY ?? e.changedTouches?.[0]?.clientY ?? e.touches?.[0]?.clientY;
    if (clientX == null) return;
    const sx = clientX - rect.left;
    const sy = clientY - rect.top;
    game.screenMouse = { x: sx, y: sy };
    game.worldMouse = game.screenToWorld(sx, sy);

    if (type === "move") {
      if (game.dragCam) {
        const dx = sx - game.dragCam.x;
        const dy = sy - game.dragCam.y;
        game.cam.x = game.dragCam.cx - dx;
        game.cam.y = game.dragCam.cy - dy;
      } else if (game.running) {
        game.hoverCell = game.cellAtWorld(game.worldMouse.x, game.worldMouse.y);
      }
      return;
    }

    if (type === "down") {
      if (e.button === 1 || e.button === 2 || e.shiftKey) {
        game.dragCam = { x: sx, y: sy, cx: game.cam.x, cy: game.cam.y };
      }
      return;
    }

    if (type === "up") {
      if (game.dragCam) {
        game.dragCam = null;
        return;
      }
      if (!game.running || game.paused) return;
      if (e.target !== canvas) return;

      const cell = game.cellAtWorld(game.worldMouse.x, game.worldMouse.y);
      if (cell) {
        game.selectCell(cell);
      } else {
        game.hideActionPanel();
      }
    }
  }

  canvas.addEventListener("mousedown", (e) => onPointer(e, "down"));
  window.addEventListener("mouseup", (e) => onPointer(e, "up"));
  window.addEventListener("mousemove", (e) => onPointer(e, "move"));
  canvas.addEventListener("touchstart", (e) => onPointer(e, "down"), { passive: true });
  window.addEventListener("touchend", (e) => onPointer(e, "up"), { passive: true });
  window.addEventListener("touchmove", (e) => onPointer(e, "move"), { passive: true });
  canvas.addEventListener("contextmenu", (e) => e.preventDefault());

  window.addEventListener("keydown", (e) => {
    if (e.key === " " || e.key === "p") {
      e.preventDefault();
      if (game.running) game.paused = !game.paused;
    }
    if (e.key === "Escape") game.hideActionPanel();
  });

  let last = performance.now();
  function frame(now) {
    const dt = Math.min(0.05, (now - last) / 1000);
    last = now;
    game.update(dt);
    game.draw();
    requestAnimationFrame(frame);
  }
  requestAnimationFrame(frame);
})();

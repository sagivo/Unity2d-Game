/**
 * Asset registry + persistence for the settings editor.
 * Overrides (data URLs) and scales live in localStorage.
 */

export const STORAGE_KEY = "hexcore-asset-settings-v1";

/** User-tunable gameplay knobs (defaults = easy mode) */
export const DEFAULT_BALANCE = {
  startGold: 50,
  graceSeconds: 18,
  waveSeconds: 32,
  spawnInterval: 6.5,
  spawnIntervalMin: 1.8,
  spawnCount: 1,
  extraSpawnChance: 0.2,
  coreHp: 300,
  coreDamage: 12,
  coreFireRate: 0.45,
  turretCost: 8,
  turretDamage: 14,
  turretHp: 100,
  turretFireRate: 0.7,
  minerCost: 12,
  minerHp: 60,
  minerInterval: 1.4,
  expandCost: 15,
  enemyHpMult: 1,
  enemyDamageMult: 1,
  enemySpeedMult: 1,
  killGoldMult: 1,
};

/** UI field definitions for the Gameplay tab */
export const BALANCE_FIELDS = [
  {
    group: "Match flow",
    items: [
      { key: "startGold", label: "Starting gold", min: 0, max: 300, step: 5, unit: "" },
      { key: "graceSeconds", label: "Setup time", min: 0, max: 60, step: 1, unit: "s" },
      { key: "waveSeconds", label: "Wave length", min: 10, max: 90, step: 1, unit: "s" },
      { key: "spawnInterval", label: "Spawn every", min: 1, max: 15, step: 0.5, unit: "s" },
      { key: "spawnIntervalMin", label: "Fastest spawn", min: 0.5, max: 8, step: 0.1, unit: "s" },
      { key: "spawnCount", label: "Enemies per spawn", min: 1, max: 6, step: 1, unit: "" },
      { key: "extraSpawnChance", label: "Extra spawn chance", min: 0, max: 1, step: 0.05, unit: "" },
    ],
  },
  {
    group: "Your strength",
    items: [
      { key: "coreHp", label: "Core HP", min: 50, max: 2000, step: 25, unit: "" },
      { key: "coreDamage", label: "Core damage", min: 1, max: 80, step: 1, unit: "" },
      { key: "coreFireRate", label: "Core fire delay", min: 0.1, max: 2, step: 0.05, unit: "s" },
      { key: "turretCost", label: "Turret cost", min: 1, max: 80, step: 1, unit: "" },
      { key: "turretDamage", label: "Turret damage", min: 1, max: 100, step: 1, unit: "" },
      { key: "turretHp", label: "Turret HP", min: 20, max: 500, step: 5, unit: "" },
      { key: "turretFireRate", label: "Turret fire delay", min: 0.15, max: 2.5, step: 0.05, unit: "s" },
      { key: "minerCost", label: "Miner cost", min: 1, max: 100, step: 1, unit: "" },
      { key: "minerHp", label: "Miner HP", min: 10, max: 400, step: 5, unit: "" },
      { key: "minerInterval", label: "Gold every", min: 0.3, max: 5, step: 0.1, unit: "s" },
      { key: "expandCost", label: "Expand cost", min: 0, max: 100, step: 1, unit: "" },
    ],
  },
  {
    group: "Enemy strength",
    items: [
      { key: "enemyHpMult", label: "Enemy HP", min: 0.25, max: 4, step: 0.05, unit: "×" },
      { key: "enemyDamageMult", label: "Enemy damage", min: 0.25, max: 4, step: 0.05, unit: "×" },
      { key: "enemySpeedMult", label: "Enemy speed", min: 0.25, max: 3, step: 0.05, unit: "×" },
      { key: "killGoldMult", label: "Kill gold", min: 0, max: 5, step: 0.1, unit: "×" },
    ],
  },
];

function scaleArr(arr, mult, asInt = true) {
  return arr.map((v) => {
    const n = v * mult;
    return asInt ? Math.max(1, Math.round(n)) : Math.max(0.05, +n.toFixed(3));
  });
}

function costLadder(base) {
  const b = Math.max(0, Math.round(base));
  return [b, Math.round(b * 1.5), Math.round(b * 2.5), Math.round(b * 4)];
}

function refundLadder(base) {
  return costLadder(base).map((v) => Math.max(0, Math.round(v * 0.5)));
}

/**
 * Build runtime BALANCE object from saved knobs.
 */
export function buildBalance(settings) {
  const b = { ...DEFAULT_BALANCE, ...(settings?.balance || {}) };

  const coreHp = Math.round(b.coreHp);
  const coreDmg = Math.round(b.coreDamage);
  const tCost = Math.round(b.turretCost);
  const tDmg = Math.round(b.turretDamage);
  const tHp = Math.round(b.turretHp);
  const tFire = b.turretFireRate;
  const mCost = Math.round(b.minerCost);
  const mHp = Math.round(b.minerHp);
  const mInt = b.minerInterval;
  const eCost = Math.round(b.expandCost);

  const hpM = b.enemyHpMult;
  const dmgM = b.enemyDamageMult;
  const spdM = b.enemySpeedMult;
  const goldM = b.killGoldMult;

  return {
    startMinerals: Math.round(b.startGold),
    gracePeriod: b.graceSeconds,
    waveSeconds: b.waveSeconds,
    spawnInterval: b.spawnInterval,
    spawnIntervalMin: b.spawnIntervalMin,
    spawnCount: Math.max(1, Math.round(b.spawnCount)),
    extraSpawnChance: b.extraSpawnChance,
    controls: b,
    player: {
      core: {
        healthPerLevel: [coreHp, Math.round(coreHp * 1.7), coreHp * 20, coreHp * 40],
        damagePerLevel: [coreDmg, Math.round(coreDmg * 1.5), Math.round(coreDmg * 2), Math.round(coreDmg * 3.5)],
        fireRate: b.coreFireRate,
        bulletSpeed: 420,
        autoRange: 420,
      },
      autoCanon: {
        buildCostPerLevel: costLadder(tCost),
        refundPerLevel: refundLadder(tCost),
        shootSpeedPerLevel: [tFire, tFire * 0.7, tFire * 0.5, tFire * 0.35].map((v) => +v.toFixed(3)),
        upgradeTimePerLevel: [0.4, 1.2, 2, 3],
        healthPerLevel: [tHp, Math.round(tHp * 1.4), Math.round(tHp * 1.9), Math.round(tHp * 2.5)],
        damagePerLevel: [tDmg, Math.round(tDmg * 1.4), Math.round(tDmg * 2.1), Math.round(tDmg * 3.2)],
        range: 300,
        bulletSpeed: 400,
      },
      mineralMiner: {
        buildCostPerLevel: costLadder(mCost),
        refundPerLevel: refundLadder(mCost),
        upgradeTimePerLevel: [0.5, 1.5, 2.5, 4],
        timeToMineralPerLevel: [mInt, mInt * 0.7, mInt * 0.5, mInt * 0.35].map((v) => +Math.max(0.2, v).toFixed(3)),
        healthPerLevel: [mHp, Math.round(mHp * 1.5), Math.round(mHp * 2), Math.round(mHp * 2.7)],
      },
      cell: {
        expendCostPerLevel: [eCost, Math.round(eCost * 1.3), Math.round(eCost * 2), 1],
      },
    },
    enemy: {
      kamikazi: {
        healthPerLevel: scaleArr([28, 50, 75], hpM),
        damagePerLevel: scaleArr([6, 10, 16], dmgM),
        speedPerLevel: scaleArr([38, 48, 58], spdM, false).map((v) => +v.toFixed(1)),
        killBonus: scaleArr([2, 4, 6], goldM),
      },
      destroyer: {
        healthPerLevel: scaleArr([70, 120, 180], hpM),
        damagePerLevel: scaleArr([5, 9, 14], dmgM),
        speedPerLevel: scaleArr([28, 34, 40], spdM, false).map((v) => +v.toFixed(1)),
        rangePerLevel: [150, 180, 210],
        shootSpeedPerLevel: [1.8, 1.4, 1.1],
        killBonus: scaleArr([4, 7, 11], goldM),
      },
      orc: {
        healthPerLevel: scaleArr([55, 95, 140], hpM),
        damagePerLevel: scaleArr([8, 14, 20], dmgM),
        speedPerLevel: scaleArr([30, 36, 42], spdM, false).map((v) => +v.toFixed(1)),
        attackSpeedPerLevel: [1.5, 1.2, 1.0],
        meleeRange: 36,
        killBonus: scaleArr([3, 6, 9], goldM),
      },
      goblin: {
        healthPerLevel: scaleArr([35, 60, 95], hpM),
        damagePerLevel: scaleArr([4, 8, 12], dmgM),
        speedPerLevel: scaleArr([34, 42, 50], spdM, false).map((v) => +v.toFixed(1)),
        rangePerLevel: [140, 170, 200],
        attackSpeedPerLevel: [1.6, 1.3, 1.0],
        killBonus: scaleArr([3, 5, 8], goldM),
      },
    },
  };
}

/** Default paths shipped with the game */
export const DEFAULT_PATHS = {
  cell: "assets/cell_tinted.png",
  cellInactive: "assets/cell_inactive.png",
  cellSelected: "assets/cell_selected.png",
  factory: "assets/buildings/Factory.png",
  cannon: "assets/player/cannon1.png",
  cannonBarrel: "assets/player/cannon2.png",
  playerIcon: "assets/player/palyer.png",
  resource: "assets/player/resource1.png",
  bullet: "assets/fx/bullet.png",
  duck: "assets/fx/rubber-duck.png",
  orc: "assets/enemies/orc_full.png",
  goblin: "assets/enemies/goblin_full.png",
  fighter: "assets/enemies/fighter.png",
  bomber: "assets/enemies/bomberHull.png",
  hpBg: "assets/ui/HP BG.png",
  hpBar: "assets/ui/HP Bar.png",
  baby0: "assets/player/baby_frames/baby_0.png",
  baby1: "assets/player/baby_frames/baby_1.png",
  baby2: "assets/player/baby_frames/baby_2.png",
  baby3: "assets/player/baby_frames/baby_3.png",
  baby4: "assets/player/baby_frames/baby_4.png",
  baby5: "assets/player/baby_frames/baby_5.png",
  baby6: "assets/player/baby_frames/baby_6.png",
  baby7: "assets/player/baby_frames/baby_7.png",
  kami_sheet: "assets/enemies/DemonFighter.png",
  dest_sheet: "assets/enemies/canonDestroyer.png",
};

for (let r = 0; r < 4; r++) {
  for (let c = 0; c < 4; c++) {
    DEFAULT_PATHS[`kami_${r}_${c}`] = `assets/enemies/kamikazi/${r}_${c}.png`;
    DEFAULT_PATHS[`dest_${r}_${c}`] = `assets/enemies/destroyer/${r}_${c}.png`;
  }
}

/** Editable slots shown in the settings UI */
export const ASSET_SLOTS = [
  {
    group: "Base",
    items: [
      { key: "cell", label: "Hex tile", desc: "Normal empty tile" },
      { key: "cellSelected", label: "Selected tile", desc: "When a tile is selected" },
      { key: "cellInactive", label: "Used tile", desc: "After expand" },
    ],
  },
  {
    group: "Player",
    items: [
      { key: "baby0", label: "Core (main)", desc: "Baby core sprite — used as default pose" },
      { key: "baby2", label: "Core (alt pose)", desc: "Optional second core frame" },
      { key: "cannon", label: "Turret body", desc: "Auto-turret base icon" },
      { key: "cannonBarrel", label: "Turret barrel", desc: "Rotating gun barrel" },
      { key: "factory", label: "Miner / factory", desc: "Gold producer building" },
      { key: "bullet", label: "Bullet", desc: "Player & turret shots" },
    ],
  },
  {
    group: "Enemies",
    items: [
      { key: "kami_0_0", label: "Kamikazi", desc: "Flying suicide enemy" },
      { key: "dest_0_0", label: "Destroyer", desc: "Ranged bird enemy" },
      { key: "orc", label: "Orc", desc: "Melee bruiser" },
      { key: "goblin", label: "Goblin", desc: "Archer enemy" },
      { key: "fighter", label: "Fallback enemy", desc: "Used if other art fails" },
    ],
  },
  {
    group: "Extras",
    items: [
      { key: "duck", label: "Rubber duck", desc: "Fun projectile / deco" },
      { key: "playerIcon", label: "Player icon", desc: "Legacy icon" },
      { key: "resource", label: "Resource icon", desc: "Legacy resource sprite" },
    ],
  },
];

/** Logical draw sizes (base px). Scale setting multiplies these. */
export const DRAW_SIZES = {
  cell: 100,
  core: 72,
  coreH: 90,
  turret: 56,
  barrelW: 34,
  barrelH: 20,
  miner: 72,
  minerH: 64,
  kamikaziW: 64,
  kamikaziH: 40,
  destroyerW: 72,
  destroyerH: 48,
  orcW: 60,
  orcH: 70,
  goblinW: 56,
  goblinH: 64,
  bullet: 10,
};

function defaultScales() {
  return {
    cell: 1,
    core: 1,
    turret: 1,
    miner: 1,
    kamikazi: 1,
    destroyer: 1,
    orc: 1,
    goblin: 1,
    bullet: 1,
  };
}

export function defaultSettings() {
  return {
    overrides: {},
    scales: defaultScales(),
    hideLabels: false,
    balance: { ...DEFAULT_BALANCE },
  };
}

export function loadSettings() {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return defaultSettings();
    const data = JSON.parse(raw);
    return {
      overrides: data.overrides || {},
      scales: { ...defaultScales(), ...(data.scales || {}) },
      hideLabels: !!data.hideLabels,
      balance: { ...DEFAULT_BALANCE, ...(data.balance || {}) },
    };
  } catch {
    return defaultSettings();
  }
}

export function saveSettings(settings) {
  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify({
      overrides: settings.overrides || {},
      scales: settings.scales || defaultScales(),
      hideLabels: !!settings.hideLabels,
      balance: { ...DEFAULT_BALANCE, ...(settings.balance || {}) },
    })
  );
}

export function clearSettings() {
  localStorage.removeItem(STORAGE_KEY);
}

export function getAssetSrc(key, settings) {
  if (settings?.overrides?.[key]) return settings.overrides[key];
  return DEFAULT_PATHS[key] || null;
}

export function loadImage(src) {
  return new Promise((resolve, reject) => {
    const img = new Image();
    img.onload = () => resolve(img);
    img.onerror = () => reject(new Error("Failed to load " + src));
    img.src = src;
  });
}

export async function loadAllAssets(settings) {
  const s = settings || loadSettings();
  const keys = Object.keys(DEFAULT_PATHS);
  const entries = await Promise.all(
    keys.map(async (k) => {
      const src = getAssetSrc(k, s);
      try {
        return [k, await loadImage(src)];
      } catch {
        // fall back to default path if override broken
        if (s.overrides?.[k] && DEFAULT_PATHS[k]) {
          try {
            return [k, await loadImage(DEFAULT_PATHS[k])];
          } catch {
            return [k, null];
          }
        }
        return [k, null];
      }
    })
  );
  const assets = Object.fromEntries(entries);
  assets.__settings = s;
  return assets;
}

export function fileToDataURL(file) {
  return new Promise((resolve, reject) => {
    if (!file || !file.type.startsWith("image/")) {
      reject(new Error("Please choose an image file"));
      return;
    }
    if (file.size > 2.5 * 1024 * 1024) {
      reject(new Error("Image too large (max 2.5MB)"));
      return;
    }
    const reader = new FileReader();
    reader.onload = () => resolve(reader.result);
    reader.onerror = () => reject(new Error("Could not read file"));
    reader.readAsDataURL(file);
  });
}

/** When core main is replaced, mirror to all baby frames for simple editing */
export function applyCoreOverride(settings, dataUrl) {
  const next = { ...settings, overrides: { ...settings.overrides } };
  for (let i = 0; i < 8; i++) next.overrides[`baby${i}`] = dataUrl;
  return next;
}

/** When kamikazi main frame replaced, fill all anim frames */
export function applySheetOverride(settings, prefix, dataUrl) {
  const next = { ...settings, overrides: { ...settings.overrides } };
  for (let r = 0; r < 4; r++) {
    for (let c = 0; c < 4; c++) {
      next.overrides[`${prefix}_${r}_${c}`] = dataUrl;
    }
  }
  return next;
}

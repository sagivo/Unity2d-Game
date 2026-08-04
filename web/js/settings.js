import {
  ASSET_SLOTS,
  BALANCE_FIELDS,
  DEFAULT_BALANCE,
  loadSettings,
  saveSettings,
  clearSettings,
  getAssetSrc,
  fileToDataURL,
  applyCoreOverride,
  applySheetOverride,
  loadAllAssets,
  buildBalance,
} from "./assets-config.js";

/**
 * Settings page — Gameplay + Assets tabs.
 * @param {{ onSettingsApplied: (payload: { assets, balance, settings }) => void, getGame: () => any }} opts
 */
export function initSettings(opts) {
  const root = document.getElementById("settings");
  const grid = document.getElementById("settings-grid");
  const fileInput = document.getElementById("settings-file");
  const titleEl = document.getElementById("settings-title");
  const subEl = document.getElementById("settings-sub");
  let settings = loadSettings();
  let activeKey = null;
  let dirty = false;
  let tab = "gameplay"; // gameplay | assets

  function open(preferredTab) {
    settings = loadSettings();
    if (!settings.balance) settings.balance = { ...DEFAULT_BALANCE };
    dirty = false;
    if (preferredTab) tab = preferredTab;
    root.classList.remove("hidden");
    render();
    const game = opts.getGame?.();
    if (game?.running) game.paused = true;
  }

  function close() {
    root.classList.add("hidden");
  }

  async function applyAndReload() {
    saveSettings(settings);
    const assets = await loadAllAssets(settings);
    const balance = buildBalance(settings);
    opts.onSettingsApplied?.({ assets, balance, settings });
    dirty = false;
    const status = document.getElementById("settings-status");
    if (status) {
      status.textContent = "Saved — gameplay & assets applied";
      setTimeout(() => {
        if (status.textContent.startsWith("Saved")) status.textContent = "";
      }, 2200);
    }
  }

  function setTab(next) {
    tab = next;
    render();
  }

  function formatVal(key, v, unit) {
    if (unit === "×") return `${Number(v).toFixed(2)}×`;
    if (unit === "s") return `${Number(v)}s`;
    if (key === "extraSpawnChance") return `${Math.round(Number(v) * 100)}%`;
    if (Number.isInteger(Number(v)) || (Number(v) * 10) % 1 === 0) return String(Number(v));
    return Number(v).toFixed(2);
  }

  function renderTabs() {
    const nav = document.createElement("div");
    nav.className = "settings-tabs";
    nav.innerHTML = `
      <button type="button" class="stab ${tab === "gameplay" ? "active" : ""}" data-tab="gameplay">Gameplay</button>
      <button type="button" class="stab ${tab === "assets" ? "active" : ""}" data-tab="assets">Assets</button>`;
    nav.querySelectorAll("[data-tab]").forEach((btn) => {
      btn.addEventListener("click", () => setTab(btn.dataset.tab));
    });
    return nav;
  }

  function renderGameplay() {
    if (titleEl) titleEl.textContent = "Game Settings";
    if (subEl) subEl.textContent = "Tune difficulty, damage, spawns, and economy.";

    const presets = document.createElement("section");
    presets.className = "settings-section";
    presets.innerHTML = `<h3>Presets</h3>`;
    const row = document.createElement("div");
    row.className = "preset-row";
    const mk = (name, patch) => {
      const b = document.createElement("button");
      b.type = "button";
      b.className = "btn-sm";
      b.textContent = name;
      b.addEventListener("click", () => {
        settings.balance = { ...DEFAULT_BALANCE, ...patch };
        dirty = true;
        render();
      });
      return b;
    };
    row.append(
      mk("Easy", {}),
      mk("Normal", {
        startGold: 35,
        graceSeconds: 12,
        coreHp: 220,
        coreDamage: 10,
        turretDamage: 12,
        enemyHpMult: 1.25,
        enemyDamageMult: 1.2,
        enemySpeedMult: 1.1,
        spawnInterval: 5.2,
        spawnCount: 1,
        extraSpawnChance: 0.3,
      }),
      mk("Hard", {
        startGold: 25,
        graceSeconds: 8,
        coreHp: 160,
        coreDamage: 8,
        turretDamage: 10,
        turretCost: 10,
        minerCost: 14,
        enemyHpMult: 1.7,
        enemyDamageMult: 1.6,
        enemySpeedMult: 1.35,
        spawnInterval: 4,
        spawnIntervalMin: 1.2,
        spawnCount: 2,
        extraSpawnChance: 0.4,
        killGoldMult: 0.8,
      }),
      mk("Chaos", {
        startGold: 80,
        graceSeconds: 5,
        coreHp: 400,
        coreDamage: 20,
        turretDamage: 22,
        enemyHpMult: 1.1,
        enemyDamageMult: 1.4,
        enemySpeedMult: 1.6,
        spawnInterval: 2.5,
        spawnIntervalMin: 0.8,
        spawnCount: 3,
        extraSpawnChance: 0.55,
        killGoldMult: 1.5,
      })
    );
    presets.appendChild(row);
    grid.appendChild(presets);

    for (const group of BALANCE_FIELDS) {
      const sec = document.createElement("section");
      sec.className = "settings-section";
      sec.innerHTML = `<h3>${group.group}</h3>`;
      const list = document.createElement("div");
      list.className = "balance-grid";

      for (const item of group.items) {
        const val = settings.balance[item.key] ?? DEFAULT_BALANCE[item.key];
        const lab = document.createElement("label");
        lab.className = "balance-row";
        lab.innerHTML = `
          <span class="b-label">${item.label}</span>
          <input type="range" min="${item.min}" max="${item.max}" step="${item.step}"
            value="${val}" data-balance="${item.key}" />
          <em data-balance-val="${item.key}">${formatVal(item.key, val, item.unit)}</em>`;
        list.appendChild(lab);
      }
      sec.appendChild(list);
      grid.appendChild(sec);
    }

    grid.querySelectorAll("input[data-balance]").forEach((input) => {
      input.addEventListener("input", () => {
        const k = input.dataset.balance;
        const field = BALANCE_FIELDS.flatMap((g) => g.items).find((i) => i.key === k);
        let v = parseFloat(input.value);
        if (field && field.step >= 1 && field.step % 1 === 0) v = Math.round(v);
        settings.balance[k] = v;
        const em = grid.querySelector(`[data-balance-val="${k}"]`);
        if (em) em.textContent = formatVal(k, v, field?.unit || "");
        dirty = true;
      });
    });
  }

  function renderAssets() {
    if (titleEl) titleEl.textContent = "Asset Settings";
    if (subEl) subEl.textContent = "Replace sprites and tweak on-screen sizes.";

    const scalesSec = document.createElement("section");
    scalesSec.className = "settings-section";
    scalesSec.innerHTML = `<h3>Sprite sizes</h3><p class="settings-hint">Scale how big each thing appears in-game.</p>`;
    const scalesRow = document.createElement("div");
    scalesRow.className = "scale-grid";
    const scaleKeys = [
      ["cell", "Tiles"],
      ["core", "Core"],
      ["turret", "Turrets"],
      ["miner", "Miners"],
      ["kamikazi", "Kamikazi"],
      ["destroyer", "Destroyer"],
      ["orc", "Orc"],
      ["goblin", "Goblin"],
      ["bullet", "Bullets"],
    ];
    for (const [key, label] of scaleKeys) {
      const val = settings.scales[key] ?? 1;
      const row = document.createElement("label");
      row.className = "scale-row";
      row.innerHTML = `
        <span>${label}</span>
        <input type="range" min="0.4" max="2.2" step="0.05" value="${val}" data-scale="${key}" />
        <em data-scale-val="${key}">${val.toFixed(2)}×</em>`;
      scalesRow.appendChild(row);
    }
    scalesSec.appendChild(scalesRow);
    grid.appendChild(scalesSec);

    const optSec = document.createElement("section");
    optSec.className = "settings-section";
    optSec.innerHTML = `<h3>Display</h3>`;
    const chk = document.createElement("label");
    chk.className = "check-row";
    chk.innerHTML = `<input type="checkbox" id="opt-hide-labels" ${settings.hideLabels ? "checked" : ""}/> Hide building labels (Turret / Miner text)`;
    optSec.appendChild(chk);
    grid.appendChild(optSec);

    for (const group of ASSET_SLOTS) {
      const sec = document.createElement("section");
      sec.className = "settings-section";
      sec.innerHTML = `<h3>${group.group}</h3>`;
      const list = document.createElement("div");
      list.className = "asset-list";

      for (const item of group.items) {
        const src = getAssetSrc(item.key, settings);
        const overridden = !!settings.overrides[item.key];
        const card = document.createElement("div");
        card.className = "asset-card" + (overridden ? " overridden" : "");
        card.innerHTML = `
          <div class="asset-preview">
            <img src="${src}" alt="${item.label}" />
          </div>
          <div class="asset-info">
            <div class="asset-name">${item.label}${overridden ? ' <span class="tag">custom</span>' : ""}</div>
            <div class="asset-desc">${item.desc}</div>
            <div class="asset-actions">
              <button type="button" class="btn-sm" data-act="replace" data-key="${item.key}">Replace</button>
              <button type="button" class="btn-sm ghost" data-act="reset" data-key="${item.key}" ${overridden ? "" : "disabled"}>Reset</button>
            </div>
          </div>`;
        list.appendChild(card);
      }
      sec.appendChild(list);
      grid.appendChild(sec);
    }

    grid.querySelectorAll("input[data-scale]").forEach((input) => {
      input.addEventListener("input", () => {
        const k = input.dataset.scale;
        const v = parseFloat(input.value);
        settings.scales[k] = v;
        const em = grid.querySelector(`[data-scale-val="${k}"]`);
        if (em) em.textContent = v.toFixed(2) + "×";
        dirty = true;
      });
    });

    const hideLabels = grid.querySelector("#opt-hide-labels");
    if (hideLabels) {
      hideLabels.addEventListener("change", () => {
        settings.hideLabels = hideLabels.checked;
        dirty = true;
      });
    }

    grid.querySelectorAll("[data-act=replace]").forEach((btn) => {
      btn.addEventListener("click", () => {
        activeKey = btn.dataset.key;
        fileInput.value = "";
        fileInput.click();
      });
    });

    grid.querySelectorAll("[data-act=reset]").forEach((btn) => {
      btn.addEventListener("click", () => {
        const key = btn.dataset.key;
        delete settings.overrides[key];
        if (key === "baby0") {
          for (let i = 0; i < 8; i++) delete settings.overrides[`baby${i}`];
        }
        if (key === "kami_0_0") {
          for (let r = 0; r < 4; r++)
            for (let c = 0; c < 4; c++) delete settings.overrides[`kami_${r}_${c}`];
        }
        if (key === "dest_0_0") {
          for (let r = 0; r < 4; r++)
            for (let c = 0; c < 4; c++) delete settings.overrides[`dest_${r}_${c}`];
        }
        dirty = true;
        render();
      });
    });
  }

  function render() {
    grid.innerHTML = "";
    grid.appendChild(renderTabs());
    if (tab === "gameplay") renderGameplay();
    else renderAssets();
  }

  fileInput.addEventListener("change", async () => {
    const file = fileInput.files?.[0];
    if (!file || !activeKey) return;
    const status = document.getElementById("settings-status");
    try {
      const dataUrl = await fileToDataURL(file);
      if (activeKey === "baby0") settings = applyCoreOverride(settings, dataUrl);
      else if (activeKey === "kami_0_0") settings = applySheetOverride(settings, "kami", dataUrl);
      else if (activeKey === "dest_0_0") settings = applySheetOverride(settings, "dest", dataUrl);
      else {
        settings = {
          ...settings,
          overrides: { ...settings.overrides, [activeKey]: dataUrl },
        };
      }
      dirty = true;
      if (status) status.textContent = `Replaced ${activeKey} — click Apply`;
      render();
    } catch (err) {
      if (status) status.textContent = err.message || "Upload failed";
    }
    activeKey = null;
  });

  document.getElementById("btn-settings-open")?.addEventListener("click", () => open("gameplay"));
  document.getElementById("btn-settings-hud")?.addEventListener("click", () => open("gameplay"));
  document.getElementById("btn-settings-close")?.addEventListener("click", () => {
    if (dirty && !confirm("Discard unsaved changes?")) return;
    close();
  });
  document.getElementById("btn-settings-apply")?.addEventListener("click", async () => {
    await applyAndReload();
  });
  document.getElementById("btn-settings-reset-all")?.addEventListener("click", async () => {
    if (!confirm("Reset ALL settings (gameplay + assets) to defaults?")) return;
    clearSettings();
    settings = loadSettings();
    dirty = true;
    render();
    await applyAndReload();
  });

  document.getElementById("btn-settings-export")?.addEventListener("click", () => {
    const blob = new Blob([JSON.stringify(settings, null, 2)], { type: "application/json" });
    const a = document.createElement("a");
    a.href = URL.createObjectURL(blob);
    a.download = "hexcore-settings.json";
    a.click();
    URL.revokeObjectURL(a.href);
  });

  document.getElementById("btn-settings-import")?.addEventListener("click", () => {
    document.getElementById("settings-import-file").click();
  });

  document.getElementById("settings-import-file")?.addEventListener("change", async (e) => {
    const file = e.target.files?.[0];
    if (!file) return;
    try {
      const text = await file.text();
      const data = JSON.parse(text);
      const base = loadSettings();
      settings = {
        overrides: data.overrides || {},
        scales: { ...base.scales, ...(data.scales || {}) },
        hideLabels: !!data.hideLabels,
        balance: { ...DEFAULT_BALANCE, ...(data.balance || {}) },
      };
      dirty = true;
      render();
      document.getElementById("settings-status").textContent = "Imported — click Apply";
    } catch {
      document.getElementById("settings-status").textContent = "Invalid settings file";
    }
    e.target.value = "";
  });

  window.addEventListener("keydown", (e) => {
    if (e.key === "Escape" && !root.classList.contains("hidden")) {
      if (!document.getElementById("action-panel")?.classList.contains("hidden")) return;
      close();
    }
  });

  return { open, close, getSettings: () => settings };
}

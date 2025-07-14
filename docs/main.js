const {createApp} = Vue;
const {createRouter, createWebHashHistory} = VueRouter;

async function fetchData(url) {
    try {
        const response = await fetch(url);
        if (!response.ok) {
            return null;
        }

        return await response.json();
    } catch (error) {
        return null;
    }
}

function byIndex(a, b) {
    return a.index - b.index;
}

function filterWeapons(values, id, name, type, subType) {
    return Object.keys(values)
        .map(key => values[key])
        .filter(weapon => !id || String(weapon.id).includes(id))
        .filter(weapon => !name || weapon.name.includes(name))
        .filter(weapon => !type || weapon.weaponType === type)
        .filter(weapon => !subType || weapon.weaponSubTypes.includes(subType))
        .sort(byIndex);
}

// --- Info box component ---
const InfoBox = {
    template: `
              <div class="info-box" :class="{ 'error': isError }">
                  <p>{{ infoMessage }}</p>
              </div>
              `,
    props: {
        infoMessage: {
            type: String,
            default: 'No info message.'
        },
        isError: false
    }
};

// --- Weapon filters ---
const WeaponFilters = {
    template: `
              <div class="form">
                  <div class="form-group">
                      <input type="text" v-model="weaponIdFilter" id="weapon-id" class="weapon-input" placeholder="Weapon ID" autocomplete="off">
                  </div>

                  <div class="form-group">
                      <input type="text" v-model="weaponNameFilter" id="weapon-name" class="weapon-input" placeholder="Weapon Name" autocomplete="off">
                  </div>

                  <div class="form-group">
                      <select class="weapon-dropdown" v-model="selectedWeaponType" id="weapon-type">
                          <option value="">All Weapon Type</option>
                          <option v-for="(value, key) in weaponTypes" :key="key" :value="key">{{ value }}</option>
                      </select>
                  </div>

                  <div class="form-group">
                      <select class="weapon-dropdown" v-model="selectedWeaponSubType" id="weapon-sub-type">
                          <option value="">All Weapon Subtype</option>
                          <option v-for="(value, key) in weaponSubTypes" :key="key" :value="key">{{ value }}</option>
                      </select>
                  </div>
              </div>
              `,
    props: {
        weaponId: {
            type: String,
            default: '',
        },
        weaponName: {
            type: String,
            default: ''
        },
        weaponType: {
            type: String,
            default: ''
        },
        weaponSubType: {
            type: String,
            default: ''
        }
    },
    data() {
        const weaponTypes = {
            'Weapon': 'Range Weapon',
            'Close': 'Melee Weapon',
            'HangShoulder': 'Hang Shoulder Weapon',
        };

        const weaponSubTypes = {
            'Kinetic': 'Kinetic Weapon',
            'Sniper': 'Sniper Weapon',
            'MachineGun': 'Machine Gun',
            'GuideRail': 'Guide Rail Weapon',
            'Explosive': 'Explosive Weapon',
            'Plasma': 'Plasma Weapon',
            'Spreadshot': 'Spreadshot Weapon',
            'Spraying': 'Spraying Weapon',
            'Ray': 'Ray Weapon',
            'Arc': 'Arc Weapon',
            'Magnetoelectric': 'Magnetoelectric Weapon',
            'Tech': 'Tech Weapon',

            'CloseIn': 'Close-In Weapon',
            'Boxing': 'Boxing Weapon',
            'Sword': 'Sword',
            'Axe': 'Axe',
            'Spear': 'Spear',
            'Dagger': 'Dagger',
            'Shield': 'Shield',

            'Rocket': 'Rocket Weapon',
            'EMP': 'EMP',
            'AntiAir': 'Anti-Air Weapon'
        }

        return {
            weaponIdFilter: this.weaponId,
            weaponNameFilter: this.weaponName,
            weaponTypes: weaponTypes,
            weaponSubTypes: weaponSubTypes,
            selectedWeaponType: this.weaponType,
            selectedWeaponSubType: this.weaponSubType
        }
    },
    watch: {
        weaponIdFilter(value) {
            const newValue = value.replace(/\D/g, '');
            if (newValue !== value) {
                this.weaponIdFilter = newValue;
            } else {
                this.$emit('update:weaponId', newValue);
            }
        },
        weaponNameFilter(value) {
            this.$emit('update:weaponName', value);
        },
        selectedWeaponType(value) {
            this.$emit('update:weaponType', value);
        },
        selectedWeaponSubType(value) {
            this.$emit('update:weaponSubType', value);
        }
    }
}

// --- Atlas component ---
const Atlas = {
    template: `
              <div class="atlas" v-if="images.length > 0">
                  <div
                      v-for="image in images"
                      :key="image.id"
                      class="atlas-item"
                      :title="image.id + ': ' + image.name"
                      @click="handleLeftClick(image.id)"
                      @contextmenu.prevent="handleRightClick(image.id)"
                      :style="getItemStyle()"
                      :class="getItemClasses(image.id)"
                  >
                    <div class="atlas-item-sprite" :style="getSpriteStyle(image.index)"></div>
                  </div>
              </div>
              `,
    props: {
        imageUrl: {
            type: String,
            required: true
        },
        displaySize: {
            type: Number,
            required: true
        },
        config: {
            type: Object,
            required: true,
            default: () => ({}),
        },
        images: {
            type: Array,
            required: true,
            default: () => []
        },
        selectedFirstId: {
            type: [String, Number],
            default: null
        },
        selectedSecondId: {
            type: [String, Number],
            default: null
        }
    },
    methods: {
        getItemStyle() {
            return {
                width: `${this.displaySize}px`,
                height: `${this.displaySize}px`
            };
        },
        getItemClasses(imageId) {
            return {
                'first-selected': this.selectedFirstId === imageId,
                'second-selected': this.selectedSecondId === imageId
            };
        },
        getSpriteStyle(index) {
            const scaleFactor = this.displaySize / this.config.imageSize;

            const column = index % this.config.rows;
            const row = Math.floor(index / this.config.rows);

            const x = -column * this.config.imageSize * scaleFactor;
            const y = -row * this.config.imageSize * scaleFactor;

            const atlasWidth = this.config.columns * this.config.imageSize * scaleFactor;
            const atlasHeight = this.config.rows * this.config.imageSize * scaleFactor;

            return {
                backgroundImage: `url(${this.imageUrl})`,
                width: `${this.displaySize}px`,
                height: `${this.displaySize}px`,
                backgroundPosition: `${x}px ${y}px`,
                backgroundSize: `${atlasWidth}px ${atlasHeight}px`
            };
        },
        handleLeftClick(imageId) {
            this.$emit('first-select', imageId);
        },
        handleRightClick(imageId) {
            this.$emit('second-select', imageId);
        }
    }
}

// --- Weapons page ---
const WeaponsPage = {
    template: `
              <div>
                  <weapon-filters
                      @update:weaponId="selectWeaponId"
                      @update:weaponName="selectWeaponName"
                      @update:weaponType="selectWeaponType"
                      @update:weaponSubType="selectWeaponSubType"></weapon-filters>
                  <info-box :info-message="infoMessage" :is-error="!hasWeapons"></info-box>
                  <atlas
                      v-if="hasWeapons"
                      :image-url="'assets/weapons.webp'" :config="weaponsAtlas"
                      :images="weaponsArray" :display-size="50"
                      :selected-first-id="firstWeaponId"
                      :selected-second-id="secondWeaponId"
                      @first-select="selectFirstWeapon"
                      @second-select="selectSecondWeapon"></atlas>
                  <weapon-component
                      :first-weapon="firstWeapon" :second-weapon="secondWeapon" :bullets="bullets"></weapon-component>
              </div>
              `,
    data() {
        return {
            hasWeapons: true,
            infoMessage: '',
            weaponsMap: {},
            weaponsArray: [],
            weaponsAtlas: {},
            bullets: {},
            firstWeaponId: null,
            secondWeaponId: null,
            firstWeapon: null,
            secondWeapon: null,
            weaponId: '',
            weaponName: '',
            weaponType: '',
            weaponSubType: ''
        };
    },
    async created() {
        const weapons = await fetchData('assets/weapons.json');
        const weaponsAtlas = await fetchData('assets/weapons_atlas.json');
        const bullets = await fetchData('assets/bullets.json');
        if (weapons && weaponsAtlas && bullets) {
            this.weaponsMap = weapons;
            this.weaponsArray = filterWeapons(weapons);
            this.weaponsAtlas = weaponsAtlas;
            this.bullets = bullets;
        } else {
            this.hasWeapons = false;
        }

        this.updateInfoMessage();
    },
    methods: {
        selectFirstWeapon(weaponId) {
            this.firstWeaponId = weaponId;
            this.firstWeapon = this.weaponsMap[weaponId];
        },
        selectSecondWeapon(weaponId) {
            this.secondWeaponId = weaponId;
            this.secondWeapon = this.weaponsMap[weaponId];
        },
        selectWeaponId(weaponId) {
            this.weaponId = weaponId;
            this.weaponsArray = filterWeapons(this.weaponsMap, this.weaponId, this.weaponName, this.weaponType, this.weaponSubType);
            this.updateInfoMessage();
        },
        selectWeaponName(weaponName) {
            this.weaponName = weaponName;
            this.weaponsArray = filterWeapons(this.weaponsMap, this.weaponId, this.weaponName, this.weaponType, this.weaponSubType);
            this.updateInfoMessage();
        },
        selectWeaponType(weaponType) {
            this.weaponType = weaponType;
            this.weaponsArray = filterWeapons(this.weaponsMap, this.weaponId, this.weaponName, this.weaponType, this.weaponSubType);
            this.updateInfoMessage();
        },
        selectWeaponSubType(weaponSubType) {
            this.weaponSubType = weaponSubType;
            this.weaponsArray = filterWeapons(this.weaponsMap, this.weaponId, this.weaponName, this.weaponType, this.weaponSubType);
            this.updateInfoMessage();
        },
        updateInfoMessage() {
            this.infoMessage = this.hasWeapons
                ? this.weaponsArray.length === 0
                    ? 'No weapons'
                    : 'Use the left and right mouse buttons to select a weapon.'
                : 'Failed to fetch weapons or no data received.'
        }
    },
    components: {
        'weapon-component': {
            template: `
                      <div class="data-container">
                          <div v-if="firstWeapon" class="data-box">
                              <h3>{{ firstWeapon.id }}: {{ firstWeapon.name }}</h3>
                              <p v-for="(line, index) in getDescription(firstWeapon)" :key="index">{{ line }}</p>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="secondWeapon" class="data-box">
                              <h3>{{ secondWeapon.id }}: {{ secondWeapon.name }}</h3>
                              <p v-for="(line, index) in getDescription(secondWeapon)" :key="index">{{ line }}</p>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="firstWeapon" class="data-box">
                              <h3>Weapon Properties</h3>
                              <pre style="overflow-x: auto;"><code style="white-space: pre-wrap; word-wrap: break-word;">{{ JSON.stringify(firstWeapon, null, 2) }}</code></pre>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="secondWeapon" class="data-box">
                              <h3>Weapon Properties</h3>
                              <pre style="overflow-x: auto;"><code style="white-space: pre-wrap; word-wrap: break-word;">{{ JSON.stringify(secondWeapon, null, 2) }}</code></pre>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="firstWeapon" class="data-box">
                              <h3>Bullet Properties</h3>
                              <pre style="overflow-x: auto;"><code style="white-space: pre-wrap; word-wrap: break-word;">{{ JSON.stringify(bullets[firstWeapon.bulletId], null, 2) }}</code></pre>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="secondWeapon" class="data-box">
                              <h3>Bullet Properties</h3>
                              <pre style="overflow-x: auto;"><code style="white-space: pre-wrap; word-wrap: break-word;">{{ JSON.stringify(bullets[secondWeapon.bulletId], null, 2) }}</code></pre>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                      </div>
                      `,
            props: {
                firstWeapon: {
                    type: Object,
                    default: () => null,
                },
                secondWeapon: {
                    type: Object,
                    default: () => null,
                },
                bullets: {
                    type: Map,
                    default: () => ({}),
                }
            },
            methods: {
                getDescription(weapon) {
                    return weapon.description
                        .replace(/<D(\d+)>/g, (match, p1) => {
                            const index = parseInt(p1, 10);
                            if (index >= 0 && index < weapon.functions.length) {
                                return weapon.functions[index].value;
                            } else {
                                return match;
                            }
                        })
                        .split('\\n');
                }
            }
        },
        'info-box': InfoBox,
        'weapon-filters': WeaponFilters,
        'atlas': Atlas
    }
};

// --- Enemies page ---
const EnemiesPage = {
    template: `
              <div>
                  <info-box info-message="Use the left and right mouse buttons to select an enemy."></info-box>
                  <ul>
                      <li v-for="enemy in enemies" :key="enemy.id">
                          <strong>{{ enemy.name }}</strong> - {{ enemy.health }}, {{ enemy.type }}
                      </li>
                  </ul>
                  <enemy-component></enemy-component>
              </div>
              `,
    data() {
        return {
            enemies: [
                {id: 1, name: 'Enemy', health: 100, type: 'Type'}
            ]
        };
    },
    components: {
        'enemy-component': {
            template: `
                      <div class="data-box">
                          <h3>Header</h3>
                          <p>Text</p>
                      </div>
                      `
        },
        'info-box': InfoBox
    }
};

// --- Routes ---
const routes = [
    {path: '/', redirect: '/weapons'},
    {path: '/weapons', component: WeaponsPage},
    {path: '/enemies', component: EnemiesPage}
];

// --- Create router ---
const router = createRouter({
    history: createWebHashHistory(),
    routes
});

// --- App ---
const app = createApp({
    data() {
        return {
            isLightTheme: false
        };
    },
    methods: {
        toggleTheme() {
            this.isLightTheme = !this.isLightTheme;
            document.body.classList.toggle('light-theme', this.isLightTheme);
            localStorage.setItem('theme', this.isLightTheme ? 'light' : 'dark');
        },
        loadTheme() {
            const savedTheme = localStorage.getItem('theme');
            if (savedTheme === 'light') {
                this.isLightTheme = true;
                document.body.classList.add('light-theme');
            } else {
                this.isLightTheme = false;
                document.body.classList.remove('light-theme');
            }
        },
        hideLoadingScreen() {
            const loadingScreen = document.getElementById('loading-screen');
            if (loadingScreen) {
                loadingScreen.classList.add('hidden');
                loadingScreen.addEventListener('transitionend', () => {
                    loadingScreen.remove();
                }, {once: true});
            }
        }
    },
    mounted() {
        this.loadTheme();
        this.hideLoadingScreen();
    }
});
app.use(router);
app.mount('#app');
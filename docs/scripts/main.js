const {createApp} = Vue;
const {createRouter, createWebHashHistory} = VueRouter;

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
                  <weapon-filters @update:filters="updateWeapons" :defaultValue="defaultWeaponFilters"></weapon-filters>
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
            defaultWeaponFilters: {id: '', name: '', type: '', subType: ''}
        };
    },
    async created() {
        const weapons = await Utils.fetchData('assets/weapons.json');
        const weaponsAtlas = await Utils.fetchData('assets/weapons_atlas.json');
        const bullets = await Utils.fetchData('assets/bullets.json');
        if (weapons && weaponsAtlas && bullets) {
            this.weaponsMap = weapons;
            this.weaponsAtlas = weaponsAtlas;
            this.bullets = bullets;

            this.updateWeapons(this.defaultWeaponFilters);
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
        updateWeapons(filters) {
            this.weaponsArray = Utils.filterWeapons(this.weaponsMap, filters);
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
        'info-box': Components.InfoBox,
        'weapon-filters': Components.WeaponFilters,
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
        'info-box': Components.InfoBox
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
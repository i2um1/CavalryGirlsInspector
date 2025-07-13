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

function sortMapByIndex(values) {
    return Object.keys(values)
        .map(key => values[key])
        .sort((a, b) => a.index - b.index);
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

// --- Atlas component ---
const Atlas = {
    template: `
              <div class="atlas">
                  <div
                      v-for="image in images"
                      :key="image.id"
                      class="atlas-item"
                      :style="getImageStyle(image.index)"
                      :title="image.id + ': ' + image.name"
                      @click="handleLeftClick(image.id)"
                      @contextmenu.prevent="handleRightClick(image.id)"
                      :class="{ 'first-selected': isFirstSelected(image.id), 'second-selected': isSecondSelected(image.id) }"
                  ></div>
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
        getImageStyle(index) {
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
        },
        isFirstSelected(imageId) {
            return this.selectedFirstId === imageId;
        },
        isSecondSelected(imageId) {
            return this.selectedSecondId === imageId;
        }
    }
}

// --- Weapons page ---
const WeaponsPage = {
    template: `
              <div>
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
                      :first-weapon="firstWeapon" :second-weapon="secondWeapon"></weapon-component>
              </div>
              `,
    data() {
        return {
            hasWeapons: true,
            infoMessage: '',
            weaponsMap: {},
            weaponsArray: [],
            weaponsAtlas: {},
            firstWeaponId: null,
            secondWeaponId: null,
            firstWeapon: null,
            secondWeapon: null
        };
    },
    async created() {
        const weapons = await fetchData('assets/weapons.json');
        const weaponsAtlas = await fetchData('assets/weapons_atlas.json');
        if (weapons && weaponsAtlas) {
            this.weaponsMap = weapons;
            this.weaponsArray = sortMapByIndex(weapons);
            this.weaponsAtlas = weaponsAtlas;
        } else {
            this.hasWeapons = false;
        }

        this.infoMessage = this.hasWeapons
            ? 'Use the left and right mouse buttons to select a weapon.'
            : 'Failed to fetch weapons or no data received.'
    },
    methods: {
        selectFirstWeapon(weaponId) {
            this.firstWeaponId = weaponId;
            this.firstWeapon = this.weaponsMap[weaponId];
        },
        selectSecondWeapon(weaponId) {
            this.secondWeaponId = weaponId;
            this.secondWeapon = this.weaponsMap[weaponId];
        }
    },
    components: {
        'weapon-component': {
            template: `
                      <div class="data-container">
                          <div v-if="firstWeapon" class="data-box">
                              <h3>{{ firstWeapon.id }}: {{ firstWeapon.name }}</h3>
                              <p>{{ firstWeapon.description }}</p>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="secondWeapon" class="data-box">
                              <h3>{{ secondWeapon.id }}: {{ secondWeapon.name }}</h3>
                              <p>{{ secondWeapon.description }}</p>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="firstWeapon" class="data-box">
                              <h3>Test1</h3>
                              <p>Text</p>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="secondWeapon" class="data-box">
                              <h3>Test2</h3>
                              <p>Text</p>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="firstWeapon" class="data-box">
                              <h3>Test1</h3>
                              <p>Text</p>
                          </div>
                          <div v-else class="data-box">
                              <h3>Weapon not selected</h3>
                          </div>
                          <div v-if="secondWeapon" class="data-box">
                              <h3>Test2</h3>
                              <p>Text</p>
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
                }
            }
        },
        'info-box': InfoBox,
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
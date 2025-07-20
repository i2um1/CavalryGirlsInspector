const {createApp} = Vue;
const {createRouter, createWebHashHistory} = VueRouter;

// --- Weapons page ---
const WeaponsPage = {
    template:
        `
        <div>
            <weapon-filters @update:filters="updateWeapons" :default-value="defaultWeaponFilters"></weapon-filters>
            <info-box :info-message="infoMessage" :is-error="!hasWeapons"></info-box>
            <atlas
                v-if="hasWeapons"
                :image-url="'assets/weapons.webp'" :config="weaponsAtlas"
                :images="weaponsArray" :display-size="50"
                :is-selectable="true" :selected-first-id="firstWeaponId" :selected-second-id="secondWeaponId"
                @select:first="selectFirstWeapon" @select:second="selectSecondWeapon"></atlas>
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
            this.weaponsMap = Utils.toMap(weapons);
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
            template:
                `
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
                        .split('\n');
                }
            }
        },
        'info-box': Components.InfoBox,
        'weapon-filters': Components.WeaponFilters,
        'atlas': Components.Atlas
    }
};

// --- Enemies page ---
const EnemiesPage = {
    template:
        `
        <div>
            <info-box :info-message="infoMessage" :is-error="!hasEnemies"></info-box>
            <atlas
                v-if="hasEnemies"
                :image-url="'assets/enemies.webp'" :add-gap="true" :config="enemiesAtlas"
                :images="enemiesArray" :display-size="50"
                :is-selectable="true" :selected-first-id="firstEnemyId" :selected-second-id="secondEnemyId"
                @select:first="selectFirstEnemy" @select:second="selectSecondEnemy"></atlas>
            <enemy-component
                :first-enemy="firstEnemy" :second-enemy="secondEnemy"></enemy-component>
        </div>
        `,
    data() {
        return {
            hasEnemies: true,
            infoMessage: '',
            enemiesMap: {},
            enemiesArray: [],
            enemiesAtlas: {},
            firstEnemyId: null,
            secondEnemyId: null,
            firstEnemy: null,
            secondEnemy: null,
            defaultEnemyFilters: {id: '', name: ''}
        };
    },
    async created() {
        const enemies = await Utils.fetchData('assets/enemies.json');
        const enemiesAtlas = await Utils.fetchData('assets/enemies_atlas.json');
        if (enemies && enemiesAtlas) {
            this.enemiesMap = Utils.toMap(enemies);
            this.enemiesAtlas = enemiesAtlas;

            this.updateEnemies(this.defaultEnemyFilters);
        } else {
            this.hasEnemies = false;
        }

        this.updateInfoMessage();
    },
    methods: {
        selectFirstEnemy(enemyId) {
            this.firstEnemyId = enemyId;
            this.firstEnemy = this.enemiesMap[enemyId];
        },
        selectSecondEnemy(enemyId) {
            this.secondEnemyId = enemyId;
            this.secondEnemy = this.enemiesMap[enemyId];
        },
        updateEnemies(filters) {
            this.enemiesArray = Utils.filterEnemies(this.enemiesMap, filters);
            this.updateInfoMessage();
        },
        updateInfoMessage() {
            this.infoMessage = this.hasEnemies
                ? this.enemiesArray.length === 0
                    ? 'No enemies'
                    : 'Use the left and right mouse buttons to select an enemy.'
                : 'Failed to fetch enemies or no data received.'
        }
    },
    components: {
        'enemy-component': {
            template:
                `
                <div class="data-container">
                    <div v-if="firstEnemy" class="data-box">
                        <h3>{{ firstEnemy.id }}: {{ firstEnemy.name }}</h3>
                        <p v-for="(line, index) in getDescription(firstEnemy)" :key="index">{{ line }}</p>
                    </div>
                    <div v-else class="data-box">
                        <h3>Enemy not selected</h3>
                    </div>
                    <div v-if="secondEnemy" class="data-box">
                        <h3>{{ secondEnemy.id }}: {{ secondEnemy.name }}</h3>
                        <p v-for="(line, index) in getDescription(secondEnemy)" :key="index">{{ line }}</p>
                    </div>
                    <div v-else class="data-box">
                        <h3>Enemy not selected</h3>
                    </div>
                    <div v-if="firstEnemy" class="data-box">
                        <h3>Enemy Properties</h3>
                        <pre style="overflow-x: auto;"><code style="white-space: pre-wrap; word-wrap: break-word;">{{ JSON.stringify(firstEnemy, null, 2) }}</code></pre>
                    </div>
                    <div v-else class="data-box">
                        <h3>Enemy not selected</h3>
                    </div>
                    <div v-if="secondEnemy" class="data-box">
                        <h3>Enemy Properties</h3>
                        <pre style="overflow-x: auto;"><code style="white-space: pre-wrap; word-wrap: break-word;">{{ JSON.stringify(secondEnemy, null, 2) }}</code></pre>
                    </div>
                    <div v-else class="data-box">
                        <h3>Enemy not selected</h3>
                    </div>
                </div>
                `,
            props: {
                firstEnemy: {
                    type: Object,
                    default: () => null,
                },
                secondEnemy: {
                    type: Object,
                    default: () => null,
                }
            },
            methods: {
                getDescription(enemy) {
                    return enemy.description.split('\n');
                }
            }
        },
        'info-box': Components.InfoBox,
        'atlas': Components.Atlas
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
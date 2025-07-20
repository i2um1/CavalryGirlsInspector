const Components = (function () {

    // --- Info Box ---
    const InfoBox = {
        template:
            `
            <div class="info-box" :class="{ 'error': isError }">
                <p>{{ infoMessage }}</p>
            </div>
            `,
        props: {
            infoMessage: {
                type: String,
                default: 'No info message.'
            },
            isError: {
                type: Boolean,
                default: false
            }
        }
    };

    // --- Text Filter ---
    const TextFilter = {
        template:
            `
            <input
                type="text" v-model="value" :id="id"
                class="text-filter" :placeholder="placeholder" autocomplete="off">
            `,
        props: {
            id: {
                type: String,
                required: true
            },
            defaultValue: {
                type: String,
                default: '',
            },
            placeholder: {
                type: String,
                required: true
            },
            isNumber: {
                type: Boolean,
                default: false
            }
        },
        data() {
            return {
                value: this.defaultValue
            }
        },
        watch: {
            value(newValue) {
                if (!this.isNumber) {
                    this.$emit('update:value', newValue);
                    return;
                }

                const numberValue = newValue.replace(/\D/g, '');
                if (numberValue !== newValue) {
                    this.value = numberValue;
                } else {
                    this.$emit('update:value', newValue);
                }
            }
        }
    };

    // --- Dropdown List Filter ---
    const DropdownListFilter = {
        template:
            `
            <select class="dropdown-list-filter" v-model="value" :id="id">
                <option value="">{{ defaultText }}</option>
                <option v-for="(value, key) in list" :key="key" :value="key">{{ value }}</option>
            </select>
            `,
        props: {
            id: {
                type: String,
                required: true
            },
            defaultValue: {
                type: String,
                default: '',
            },
            defaultText: {
                type: String,
                required: true
            },
            list: {
                type: Object,
                default: () => []
            }
        },
        data() {
            return {
                value: this.defaultValue
            }
        },
        watch: {
            value(newValue) {
                this.$emit('update:value', newValue);
            }
        }
    };

    // --- Weapon Filters ---
    const WeaponFilters = {
        template:
            `
            <div class="form">
                <div class="form-group">
                    <text-filter
                        id="weapon-id" placeholder="Weapon ID" isNumber="true"
                        :default-value="filters.id" @update:value="updateId"></text-filter>
                </div>
                <div class="form-group">
                    <text-filter
                        id="weapon-name" placeholder="Weapon Name"
                        :default-value="filters.name" @update:value="updateName"></text-filter>
                </div>
                <div class="form-group">
                    <dropdown-list-filter
                        id="weapon-type" default-text="All Weapon Type"
                        :list="types" :default-value="filters.type"
                        @update:value="updateType"></dropdown-list-filter>
                </div>
                <div class="form-group">
                    <dropdown-list-filter
                        id="weapon-sub-type" default-text="All Weapon Subtype"
                        :list="subTypes" :default-value="filters.subType"
                        @update:value="updateSubType"></dropdown-list-filter>
                </div>
            </div>
            `,
        props: {
            defaultValue: {
                type: Object,
                default: () => ({
                    id: '',
                    name: '',
                    type: '',
                    subType: ''
                })
            }
        },
        data() {
            return {
                types: Utils.WeaponTypes,
                subTypes: Utils.WeaponSubTypes,
                filters: {
                    id: this.defaultValue.id,
                    name: this.defaultValue.name,
                    type: this.defaultValue.type,
                    subType: this.defaultValue.subType
                }
            }
        },
        methods: {
            updateId(newValue) {
                this.filters.id = newValue;
                this.updateFilters();
            },
            updateName(newValue) {
                this.filters.name = newValue;
                this.updateFilters();
            },
            updateType(newValue) {
                this.filters.type = newValue;
                this.updateFilters();
            },
            updateSubType(newValue) {
                this.filters.subType = newValue;
                this.updateFilters();
            },
            updateFilters() {
                this.$emit('update:filters', this.filters);
            }
        },
        components: {
            "text-filter": TextFilter,
            "dropdown-list-filter": DropdownListFilter
        }
    };

    // --- Atlas ---
    const Atlas = {
        template:
            `
            <div class="atlas" v-if="images.length > 0">
                <div
                    v-for="image in images" :key="image.id" class="atlas-item"
                    :title="image.id + ': ' + image.name"
                    @click="handleLeftClick(image.id)"
                    @contextmenu.prevent="handleRightClick(image.id)"
                    :style="getItemStyle()" :class="getItemClasses(image.id)">
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
            isSelectable: {
                type: Boolean,
                default: false
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
                if (this.isSelectable) {
                    this.$emit('select:first', imageId);
                }
            },
            handleRightClick(imageId) {
                if (this.isSelectable) {
                    this.$emit('select:second', imageId);
                }
            }
        }
    }

    return {
        InfoBox: InfoBox,
        WeaponFilters: WeaponFilters,
        Atlas: Atlas
    };
})();
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
                        :defaultValue="filters.id" @update:value="updateId"></text-filter>
                </div>
                <div class="form-group">
                    <text-filter
                        id="weapon-name" placeholder="Weapon Name"
                        :defaultValue="filters.name" @update:value="updateName"></text-filter>
                </div>
                <div class="form-group">
                    <dropdown-list-filter
                        id="weapon-type" default-text="All Weapon Type"
                        :list="types" :defaultValue="filters.type"
                        @update:value="updateType"></dropdown-list-filter>
                </div>
                <div class="form-group">
                    <dropdown-list-filter
                        id="weapon-sub-type" default-text="All Weapon Subtype"
                        :list="subTypes" :defaultValue="filters.subType"
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

    return {
        InfoBox: InfoBox,
        WeaponFilters: WeaponFilters
    };
})();
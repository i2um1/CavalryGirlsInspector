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
                        :defaultValue="defaultId" @update:value="updateId"></text-filter>
                </div>
                <div class="form-group">
                    <text-filter
                        id="weapon-name" placeholder="Weapon Name"
                        :defaultValue="defaultName" @update:value="updateName"></text-filter>
                </div>
                <div class="form-group">
                    <dropdown-list-filter
                        id="weapon-type" default-text="All Weapon Type"
                        :list="weaponTypes" :defaultValue="defaultType"
                        @update:value="updateType"></dropdown-list-filter>
                </div>
                <div class="form-group">
                    <dropdown-list-filter
                        id="weapon-sub-type" default-text="All Weapon Subtype"
                        :list="weaponSubTypes" :defaultValue="defaultSubType"
                        @update:value="updateSubType"></dropdown-list-filter>
                </div>
            </div>
            `,
        props: {
            defaultId: {
                type: String,
                default: '',
            },
            defaultName: {
                type: String,
                default: ''
            },
            defaultType: {
                type: String,
                default: ''
            },
            defaultSubType: {
                type: String,
                default: ''
            }
        },
        data() {
            return {
                weaponTypes: Utils.WeaponTypes,
                weaponSubTypes: Utils.WeaponSubTypes
            }
        },
        methods: {
            updateId(newValue) {
                this.$emit('update:id', newValue);
            },
            updateName(newValue) {
                this.$emit('update:name', newValue);
            },
            updateType(newValue) {
                this.$emit('update:type', newValue);
            },
            updateSubType(newValue) {
                this.$emit('update:sub-type', newValue);
            }
        },
        components: {
            "text-filter": TextFilter,
            "dropdown-list-filter": DropdownListFilter
        }
    };

    return {
        InfoBox: InfoBox,
        TextFilter: TextFilter,
        DropdownListFilter: DropdownListFilter,
        WeaponFilters: WeaponFilters
    };
})();
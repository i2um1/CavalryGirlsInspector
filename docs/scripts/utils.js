const Utils = (function () {

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

    function includesCaseInsensitive(targetString, searchString) {
        const lowerTarget = String(targetString).toLowerCase();
        const lowerSearch = String(searchString).toLowerCase();

        return lowerTarget.includes(lowerSearch);
    }

    function filterWeapons(values, {id, name, type, subType}) {
        return Object.values(values)
            .filter(weapon => !id || includesCaseInsensitive(weapon.id, id))
            .filter(weapon => !name || includesCaseInsensitive(weapon.name, name))
            .filter(weapon => !type || weapon.weaponType === type)
            .filter(weapon => !subType || includesCaseInsensitive(weapon.weaponSubTypes, subType))
            .sort(byIndex);
    }

    const WeaponTypes = {
        'Weapon': 'Range Weapon',
        'Close': 'Melee Weapon',
        'HangShoulder': 'Hang Shoulder Weapon',
    };

    const WeaponSubTypes = {
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
    };

    return {
        fetchData: fetchData,
        filterWeapons: filterWeapons,

        WeaponTypes: WeaponTypes,
        WeaponSubTypes: WeaponSubTypes
    };
})();
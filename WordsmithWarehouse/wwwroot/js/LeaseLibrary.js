new Vue({
    el: '#library',
    data() {
        return{
            libraries: [],
            selectedValue: '0',
        }
    },
    created() {
        this.libraries = libraryData;
        console.log(libraryData);
        
    },
    methods: {
        getLibrary(index, prop) {
            console.log(index);
            return this.libraries[index][prop];
        },
    },
})
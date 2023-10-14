new Vue({
    el: '#library',
    data() {
        return{
            libraries: [],
            selectedValue : 1,
        }
    },
    created() {
        this.libraries = libraryData;
        console.log(libraryData);
    },
    computed: {
        
    },
    methods: {
        getLibrary(index, prop) {
            return this.libraries[index-1][prop];
        },
    }
})
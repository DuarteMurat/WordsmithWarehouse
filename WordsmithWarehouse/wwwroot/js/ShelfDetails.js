new Vue({
    el: '#shelf',
    data() {
        return{
            books: [],
        }
    },
    created() {
        this.books = bookData;
        console.log(this.books);
    },
    methods: {
        onBookClick(id) {
            window.location.assign(`/books/details/${id}`);
        },
        getBookImage(imagePath) {
            if (imagePath.charAt(0) === '~') {
                return imagePath.substring(1);
            }
            return imagePath
        },
    },
    computed: {

    },
    watch: {

    },
})
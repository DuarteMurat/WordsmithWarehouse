new Vue({
    el: '#shelfIndex',

    data() {
        return {
            shelves: [],
            shelfCount: 4,
        }
    },

    created() {
        this.shelves = shelfData;
        console.log(this.shelves);
        this.getShelvesCount();
    },
    methods: {
        getShelvesCount(bookAmount) {
            if (bookAmount != null) {
                return bookAmount.length;
            } else {
                return '0';
            }
            
        },
        onBookClick(id) {
            console.log(id)
            window.location.assign(`/books/details/${id}`);
        },
    },
    computed: {
        
    },
})
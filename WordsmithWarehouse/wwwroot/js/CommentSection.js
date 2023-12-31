﻿Vue.component('postTemplate', {
    template: '#post-template',
    data() {
        return {
            commentText: 'plaholder text',
            name: 'name',
            date: 'date',
        }
    },
})

new Vue({
    el: '#commentSection',

    data() {
        return {
            comments: [],
            user: [],
            authorId: 0,
        }
    },
    created() {
        this.comments = commentData;
        this.users = userData;
        this.userId = userId.value;
    },
    computed: {

    },
    methods: {
        getSimplifiedTime(dateCreated) {
            var date = new Date(Date.parse(dateCreated));
            return (date.getUTCDate()) + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
        },
        clearComment() {
            document.getElementById('comment').innerHTML = '';
        },
        getUserImage(imagePath) {
            if (imagePath.charAt(0) === '~') {
                return imagePath.substring(1);
            }
            return imagePath
        },
        getHours(dateCreated) {
            var date = new Date(Date.parse(dateCreated));
            return date.getHours() + ":" + (date.getMinutes()) + ":" + date.getSeconds();
        },
        getId(value) {
            $('#userId').val(value);
        },
        getAuthorId(value) {
            $('#authorId').val(value);
        },
        verifyButtons(commentId) {
            if (this.userId === commentId) {
                return true;
            }

            return false;
        },
        onDeleteCommentClick(id) {
            console.log(id)
            window.location.assign(`/Books/DeleteComment/${id}`);
        },
    }

});
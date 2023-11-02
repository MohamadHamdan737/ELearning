
    $(document).ready(function () {
        $("#ImageFile").on('change', function () {
            var image = $("#image");
            var image2 = window.URL.createObjectURL(this.files[0]);
            image.find("img").attr('src', image2);
        });
        });


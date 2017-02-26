var originalWidth;
var originalHeight;
var previewContainerWidth;
var previewContainerHeight;
var previewAspectRatio;

$('document').ready(function () {
    previewContainerWidth = $('#preview-container').width();
    previewContainerHeight = $('#preview-container').height();
    previewAspectRatio = previewContainerWidth / previewContainerHeight;

    initJcropbox();
});

function initJcropbox() {
    $('#original-image').Jcrop({
        onSelect: showPreview,
        onChange: showPreview,
        aspectRatio: previewAspectRatio
    },
        function () {
            var originalImageBounds = this.getBounds();
            originalWidth = originalImageBounds[0];
            originalHeight = originalImageBounds[1];
        });
}

function destroyJcrop() {
    var jcropApi = $('#original-image').data('Jcrop');

    if (jcropApi !== undefined) {
        jcropApi.destroy();
        $('#original-image').attr('style', "").attr("src", "");
    }
}

function initImages(input) {

    destroyJcrop();

    if (input.files && input.files[0]) {
        $('#original-image').attr('src', window.URL.createObjectURL(input.files[0]));
        $('#preview-image').attr('src', window.URL.createObjectURL(input.files[0]));
    }

    initJcropbox();
    showImagePanes();
}

function showPreview(coordinates) {

    if (parseInt(coordinates.w) > 0) {
        var rx = previewContainerWidth / coordinates.w;
        var ry = previewContainerHeight / coordinates.h;

        $('#preview-image').css({
            width: Math.round(rx * originalWidth) + 'px',
            height: Math.round(ry * originalHeight) + 'px',
            marginLeft: '-' + Math.round(rx * coordinates.x) + 'px',
            marginTop: '-' + Math.round(ry * coordinates.y) + 'px'
        });
    }
}

function showImagePanes() {
    $('.image-pane:hidden').show();
}
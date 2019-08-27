var comet = (function () {
    return {
        canvas: (function () {
            return {
                getSize: function (canvas) {
                    return { width: canvas.width, height: canvas.height };
                },
                drawImage: function (canvas, image) {
                    var binary = atob(image);
                    var array = new Uint8Array(binary.length);
                    for (var i = 0; i < binary.length; i++) {
                        array[i] = binary.charCodeAt(i);
                    }
                    var blob = new Blob([array]);

                    createImageBitmap(blob, { type: "image/png" }).then(function (img) {
                        var ctx = canvas.getContext('2d');
                        ctx.drawImage(img, 0, 0);
                    }).catch(function (error) {
                    });
                },
                moveTo: function (ctx, x, y) {
                    return ctx.moveTo(x, y);
                },
                lineTo: function (ctx, x, y) {
                    return ctx.lineTo(x, y);
                },
                fill: function (ctx) {
                    return ctx.fill();
                }
            }
        })()
    };
})();
// BookPort custom behavior
(function () {
    document.querySelectorAll('.qty-js').forEach(function (wrap) {
        var input = wrap.querySelector('input[type="number"]');
        if (!input) return;

        var clamp = function (value) {
            var min = parseInt(input.getAttribute('min') || '1', 10);
            var max = parseInt(input.getAttribute('max') || '9999', 10);
            if (Number.isNaN(value)) value = min;
            return Math.max(min, Math.min(max, value));
        };

        wrap.querySelectorAll('.qty-minus').forEach(function (btn) {
            btn.addEventListener('click', function () {
                input.value = clamp(parseInt(input.value || '1', 10) - 1);
            });
        });

        wrap.querySelectorAll('.qty-plus').forEach(function (btn) {
            btn.addEventListener('click', function () {
                input.value = clamp(parseInt(input.value || '1', 10) + 1);
            });
        });
    });

    var checkAll = document.getElementById('checkAllWishlist');
    if (checkAll) {
        checkAll.addEventListener('change', function () {
            document.querySelectorAll('.wishlist-check').forEach(function (box) {
                box.checked = checkAll.checked;
            });
        });
    }

    var mainCover = document.getElementById('mainBookCover');
    if (mainCover) {
        document.querySelectorAll('.cover-thumb').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var cover = btn.getAttribute('data-cover');
                if (cover) mainCover.setAttribute('src', cover);
                document.querySelectorAll('.cover-thumb').forEach(function (x) { x.classList.remove('active'); });
                btn.classList.add('active');
            });
        });
    }

    document.querySelectorAll('.detail-tabs [data-tab-target]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var target = btn.getAttribute('data-tab-target');
            document.querySelectorAll('.detail-tabs button').forEach(function (b) { b.classList.remove('active'); });
            document.querySelectorAll('.description-tabs .tab-panel').forEach(function (p) { p.classList.remove('active'); });
            btn.classList.add('active');
            var panel = document.getElementById(target);
            if (panel) panel.classList.add('active');
        });
    });
})();

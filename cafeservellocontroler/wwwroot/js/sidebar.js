document.getElementById('open_btn').addEventListener('click', function () {

    document.getElementById('sidebar').classList.toggle('open-sidebar');
});

document.querySelectorAll('.side-item').forEach(item => {
    item.addEventListener('click', () => {
        const url = item.getAttribute('data-url');
        window.location.href = url;
    });
});
document.querySelectorAll('.side-item-itens').forEach(item => {
    item.addEventListener('click', () => {
        const url = item.getAttribute('data-url');
        window.location.href = url;
    });
});

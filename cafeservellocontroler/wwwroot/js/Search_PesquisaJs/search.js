const searchInput = document.getElementById('search');
const table = document.getElementById('table');
const tbody = table.querySelectorAll('tbody tr');

searchInput.addEventListener('input', (event) => {
    const value = formatString(event.target.value);

    tbody.forEach(tr => {
        const textoLinha = formatString(tr.textContent);
        if (textoLinha.includes(value)) {
            tr.style.display = '';
        } else {
            tr.style.display = 'none';
        }
    });
});

function formatString(valueRecebendo) {
    return valueRecebendo
        .toLowerCase()
        .trim();
}
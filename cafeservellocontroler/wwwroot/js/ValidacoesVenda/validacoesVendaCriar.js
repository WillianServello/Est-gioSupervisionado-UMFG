// ============================================
//   GERA OPÇÕES DE PRODUTO NO SELECT
// ============================================
function gerarOpcoesProduto() {
    return produtosLista.map(p => `
        <option value="${p.id}"
                data-valor="${p.preco}"
                data-estoque="${p.estoque}">
            ${p.nome}
        </option>
    `).join("");
}

// ============================================
//   REINDEXAR ITENS DEPOIS DE REMOVER
// ============================================
function reindexarItens() {
    let index = 0;

    document.querySelectorAll('.item-row').forEach(row => {
        row.dataset.index = index;

        row.querySelector('.select-produto').name = `Itens[${index}].ProdutoId`;
        row.querySelector('.input-quantidade').name = `Itens[${index}].Quantidade`;
        row.querySelector('.input-valor-hidden').name = `Itens[${index}].Valor`;

        index++;
    });

    nextItemIndex = index;
}

// ============================================
//   CALCULAR VALOR DO ITEM
// ============================================
function calcularItem(itemRow) {
    const selectProduto = itemRow.querySelector('.select-produto');
    const inputQuantidade = itemRow.querySelector('.input-quantidade');
    const inputValorHidden = itemRow.querySelector('.input-valor-hidden');
    const spanValorDisplay = itemRow.querySelector('.valor-unitario-display');
    const spanTotalDisplay = itemRow.querySelector('.total-item-display');

    const selectedOption = selectProduto.options[selectProduto.selectedIndex];

    const valorUnitario = parseFloat(selectedOption?.getAttribute('data-valor')) || 0;
    const quantidade = parseInt(inputQuantidade.value) || 0;

    inputValorHidden.value = valorUnitario.toFixed(2);
    spanValorDisplay.innerText = valorUnitario.toFixed(2);

    const totalItem = valorUnitario * quantidade;
    spanTotalDisplay.innerText = totalItem.toFixed(2);

    atualizarTotalGeral();
}

// ============================================
//   ATUALIZA QUANDO TROCA O PRODUTO
// ============================================
function atualizarValor(select) {
    const itemRow = select.closest('.item-row');
    const selectedOption = select.options[select.selectedIndex];

    const estoque = parseInt(selectedOption?.getAttribute('data-estoque')) || 0;
    const inputQuantidade = itemRow.querySelector('.input-quantidade');

    itemRow.querySelector('.estoque-info').innerText = "Estoque: " + estoque;
    inputQuantidade.max = estoque;

    if (parseInt(inputQuantidade.value) > estoque)
        inputQuantidade.value = estoque;

    calcularItem(itemRow);
    bloquearProdutosRepetidos();
}

// ============================================
//   ATUALIZA QUANTIDADE
// ============================================
function atualizarQuantidade(input) {
    const max = parseInt(input.max);
    if (parseInt(input.value) > max) {
        input.value = max;
    }
    const itemRow = input.closest('.item-row');
    calcularItem(itemRow);
}

// ============================================
//   TOTAL GERAL
// ============================================
function atualizarTotalGeral() {
    let soma = 0;

    document.querySelectorAll('.total-item-display').forEach(span => {
        soma += parseFloat(span.innerText) || 0;
    });

    document.getElementById('totalGeral').innerText = soma.toFixed(2);
}

// ============================================
//   BLOQUEAR PRODUTOS REPETIDOS
// ============================================
function bloquearProdutosRepetidos(container = document.getElementById('itensContainer')) {

    const selects = container.querySelectorAll('.select-produto');
    const usados = [...selects].map(s => s.value).filter(v => v !== "");

    selects.forEach(s => {
        s.querySelectorAll('option').forEach(o => {
            if (usados.includes(o.value) && o.value !== s.value)
                o.disabled = true;
            else
                o.disabled = false;
        });
    });
}

// ============================================
//   ADICIONAR UM NOVO ITEM
// ============================================
function adicionarItem() {
    const container = document.getElementById('itensContainer');
    const index = nextItemIndex;

    const html = `
        <div class="row item-row mb-3 border-bottom pb-3" data-index="${index}">
            <div class="col-6">
                <label class="form-label">Produto</label>
                <select name="Itens[${index}].ProdutoId"
                        class="form-select select-produto"
                        onchange="atualizarValor(this)"
                        required>
                    <option value="">Selecione o produto</option>
                    ${gerarOpcoesProduto()}
                </select>
                <div class="text-muted estoque-info">Estoque: --</div>
            </div>

            <div class="col-3">
                <label class="form-label">Quantidade</label>
                <input type="number"
                       name="Itens[${index}].Quantidade"
                       class="form-control input-quantidade"
                       oninput="atualizarQuantidade(this)"
                       min="1"
                       value="1"
                       required />
            </div>

            <div class="col-3 text-end">
                <label class="form-label">Valor Unitário</label>
                <div class="fw-bold">
                    R$ <span class="valor-unitario-display">0.00</span>
                </div>
                <input type="hidden"
                       name="Itens[${index}].Valor"
                       class="input-valor-hidden"
                       value="0" />
                <a href="#"
                    class="text-danger remove-item"
                    onclick="removerItem(this, event)"
                    style="font-size: 0.8em;">
                    Remover
                </a>
            </div>

            <div class="col-12 mt-2 text-end">
                <label class="form-label me-2">Total Item:</label>
                <span class="fw-bold fs-5 text-primary">
                    R$ <span class="total-item-display">0.00</span>
                </span>
            </div>
        </div>
    `;

    container.insertAdjacentHTML("beforeend", html);

    nextItemIndex++;
    bloquearProdutosRepetidos();
}

// ============================================
//   REMOVER ITEM
// ============================================
function removerItem(link, event) {
    event.preventDefault();

    link.closest('.item-row').remove();

    reindexarItens();
    atualizarTotalGeral();
    bloquearProdutosRepetidos();
}

// ============================================
//   INICIALIZAÇÃO
// ============================================
document.addEventListener('DOMContentLoaded', () => {
    const container = document.querySelector(".criacao-venda");
    container.querySelectorAll('.item-row').forEach(row => calcularItem(row));
    bloquearProdutosRepetidos(container);
});

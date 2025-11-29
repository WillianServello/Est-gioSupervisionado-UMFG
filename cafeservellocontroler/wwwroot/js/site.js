//const { bottom } = require("@popperjs/core");


$('.close-alert').click(function () {
    $('.alert').hide('hide');
});

const button = document.querySelector("button");
const modal = document.querySelector("dialog");
const buttonClose = document.querySelector("dialog button");

button.onclick = function () {
    modal.showModal();
}

buttonClose.onclick = function () {
    modal.close();
}
function limparFormulario() {
    document.getElementById("formProduto").reset();

     // se você tiver mensagens de erro, pode limpar também:
     let spansErro = document.querySelectorAll(".text-danger, .field-validation-error");
     spansErro.forEach(s => s.innerText = "");
}


//abrir modal pelo detalhes
document.addEventListener("DOMContentLoaded", function () {

    document.addEventListener('click', function (e) {
        const btn = e.target.closest('.btnAbrirModal');
        if (!btn) return;

        const currentModal = document.querySelector(btn.dataset.currentModal);
        const nextModal = document.querySelector(btn.dataset.modalTarget);

        if (!currentModal || !nextModal) {
            console.warn("Modal não encontrado:", currentModal, nextModal);
            return;
        }

        const modalInstance = bootstrap.Modal.getInstance(currentModal);
        modalInstance.hide();

        currentModal.addEventListener('hidden.bs.modal', function () {
            const newModal = bootstrap.Modal.getOrCreateInstance(nextModal);
            newModal.show();
        }, { once: true });
    });

});


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



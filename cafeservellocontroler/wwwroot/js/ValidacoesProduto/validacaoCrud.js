


        const elementos = document.querySelectorAll('[data-val-number]');

        // Itera sobre a lista de elementos e remove o atributo de cada um
        elementos.forEach(elemento => {
            elemento.removeAttribute('data-val-number');
        });
 


    document.querySelectorAll('.mask-money').forEach(function (input) {
        input.addEventListener('input', function () {
            let value = input.value.replace(/\D/g, '');

            value = (value / 100).toFixed(2) + '';
            value = value.replace('.', ',');

            input.value = value;
        });
    });


$(document).ready(function () {
    $(".mask-telefone").inputmask({
        mask: ["(99) 9999-9999", "(99) 99999-9999"],
        keepStatic: true
    });
});

$(document).ready(function () {
    Inputmask({
        mask: ["999.999.999-99", "99.999.999/9999-99"],
        keepStatic: true,
        placeholder: "_"
    }).mask(".input-doc");
});

$(document).ready(function () {

    $(".toggleSenha").click(function () {
        let input = $(this).parent().find(".input-senha");
        let icon = $(this).find("i");

        if (input.attr("type") === "password") {
            input.attr("type", "text");
            icon.removeClass("fa-eye").addClass("fa-eye-slash");
        } else {
            input.attr("type", "password");
            icon.removeClass("fa-eye-slash").addClass("fa-eye");
        }
    });
});
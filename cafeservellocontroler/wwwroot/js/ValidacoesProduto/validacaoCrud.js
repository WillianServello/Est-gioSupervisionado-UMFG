


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
   
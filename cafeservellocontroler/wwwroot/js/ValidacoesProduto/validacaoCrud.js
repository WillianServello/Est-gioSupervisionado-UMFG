    $(document).ready(function () {
        const $preco = $('#preco');

        // --- segurança: garante que o plugin mask exista
        if (typeof $.fn.mask !== 'function') {
            console.error('jQuery Mask não encontrado. Verifique se jquery.mask.min.js foi incluído depois do jQuery.');
            return;
        }

        // --- aplica máscara (vai formatar enquanto digita)
        $preco.mask('000.000.000.000.000,00', {
            reverse: true,
            translation: {
                '0': { pattern: /\d/, optional: true }
            }
        });

        // --- FORÇA formatação caso já exista valor no campo ao carregar (ex: edição)
        // pega valor cru e reaplica a máscara (garante exibição correta)
        const currentVal = $preco.val();
        if (currentVal && currentVal.trim() !== '') {
            // remove tudo que não é dígito pra reformatar corretamente
            const onlyDigits = currentVal.replace(/\D/g, '');
            // se tinha somente dígitos sem casas decimais, deixa como estava para a máscara tratar
            $preco.val(onlyDigits).trigger('input');
        }

        // --- remover validação numérica do ASP.NET que conflita
        $preco.removeAttr('data-val-number');

        // --- tratamento no submit (envia em formato pt-BR: "1234,56")
        $('#formProduto').on('submit', function () {
            let valor = ($preco.val() || '').trim();
            if (!valor) return;

            // remover espaços
            valor = valor.replace(/\s/g, '');

            if (valor.indexOf(',') !== -1) {
                // já usa vírgula -> remover pontos de milhar
                valor = valor.replace(/\./g, '');
            } else if (valor.indexOf('.') !== -1) {
                // converte último ponto em vírgula se usuário usou ponto decimal
                valor = valor.replace(/\.(?=[^.]*$)/, ',');
                valor = valor.replace(/\./g, '');
            }

            $preco.val(valor);
        });

        // opcional: impedir colar texto não-numérico
        $preco.on('paste', function (e) {
            const paste = (e.originalEvent || e).clipboardData.getData('text');
            if (!/^[\d.,\s]+$/.test(paste)) {
                e.preventDefault();
            }
        });
    });



    $(document).on('shown.bs.modal', function (event) {
        const modal = $(event.target);

        // Verifica se é um modal de edição (id começa com "modalEditar-")
        const modalId = modal.attr('id');
        if (modalId && modalId.startsWith('modalEditar-')) {
            const $preco = modal.find('#precoEditar');

            if ($preco.length && typeof $.fn.mask === 'function') {
                // Aplica máscara
                $preco.mask('000.000.000.000.000,00', {
                    reverse: true,
                    translation: {
                        '0': { pattern: /\d/, optional: true }
                    }
                });

                // Força atualização visual imediata
                const valorAtual = $preco.val();
                if (valorAtual) {
                    // Remove caracteres não numéricos e reaplica
                    const valorNumerico = valorAtual.replace(/\D/g, '');
                    $preco.val(valorNumerico).trigger('input');
                }
            } else {
                console.warn('Campo #precoEditar não encontrado ou jquery.mask não carregado.');
            }
        }
        
    });


        


   
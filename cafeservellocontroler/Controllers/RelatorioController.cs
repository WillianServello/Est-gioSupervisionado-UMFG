using cafeservellocontroler.Data;
using cafeservellocontroler.Filters;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace cafeservellocontroler.Controllers
{
    [PaginaUsuarioLogado]
    public class RelatorioController : Controller
    {
        private readonly BancoContext _context;

        public RelatorioController(BancoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LucroPorProdutoIndex()
        {
            return PartialView();
        }

        public IActionResult ProdutoMaisVendidoIndex()
        {
            return PartialView();
        }

        public IActionResult RevendedorMaisAtivoIndex()
        {
            return PartialView();
        }

        public IActionResult ProdutoMaisVendido(DateTime? dataInicial, DateTime? dataFinal)
        {
            // 1. Filtrar itens pela data da venda
            var itensFiltrados =
                from item in _context.ItensVendas
                join venda in _context.Vendas
                    on EF.Property<int>(item, "Id_Venda") equals venda.Id
                where (!dataInicial.HasValue || venda.DataVenda >= dataInicial.Value)
                where (!dataFinal.HasValue || venda.DataVenda <= dataFinal.Value)
                select item;

            // 2. Agrupar produtos filtrados
            var produtosAgrupados = itensFiltrados
                .GroupBy(itemGroup => new { itemGroup.Produto.Id, itemGroup.Produto.Nome })
                .Select(group => new
                {
                    ProdutoId = group.Key.Id,
                    Nome = group.Key.Nome,
                    QuantidadeVendida = group.Sum(itemSomado => itemSomado.Quantidade)
                })
                .OrderByDescending(resultado => resultado.QuantidadeVendida)
                .ToList();

            // Fontes
            var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            using var memoryStream = new MemoryStream();
            using (var writer = new PdfWriter(memoryStream))
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                // ======= TÍTULO GRANDE ========
                var titulo = new Paragraph("Relatório - Produtos Mais Vendidos")
                    .SetFont(fontBold)
                    .SetFontSize(20)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBackgroundColor(new DeviceRgb(47, 58, 74))
                    .SetPadding(10)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginBottom(10);

                document.Add(titulo);

                // ======= PERÍODO ========
                string textoPeriodo =
                    $"Período: {(dataInicial.HasValue ? dataInicial.Value.ToString("dd/MM/yyyy") : "--")}  até  {(dataFinal.HasValue ? dataFinal.Value.ToString("dd/MM/yyyy") : "--")}";

                var periodo = new Paragraph(textoPeriodo)
                    .SetFont(fontBold)
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.BLACK)
                    .SetMarginBottom(20);

                document.Add(periodo);

                // ====== TABELA ======
                var table = new Table(2).UseAllAvailableWidth();

                var headerBackground = new DeviceRgb(47, 58, 74);

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Produto").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Quantidade Vendida").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                );

                foreach (var produto in produtosAgrupados)
                {
                    table.AddCell(
                        new Cell()
                            .Add(new Paragraph(produto.Nome).SetFont(fontNormal))
                            .SetPadding(6)
                    );

                    table.AddCell(
                        new Cell()
                            .Add(new Paragraph(produto.QuantidadeVendida.ToString()).SetFont(fontNormal).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                            .SetPadding(6)
                    );
                }

                document.Add(table);

                // ======= RODAPÉ =======
                var rodape = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFont(fontNormal)
                    .SetFontSize(11)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetMarginTop(25);

                document.Add(rodape);
            }

            return File(memoryStream.ToArray(), "application/pdf", "ProdutosMaisVendidos.pdf");
        }






        public IActionResult RevendedorMaisAtivo(DateTime? dataInicial, DateTime? dataFinal)
        {
            // JOIN manual usando VendaId
            var itensFiltrados =
                from itemVenda in _context.ItensVendas
                join venda in _context.Vendas
                    on EF.Property<int>(itemVenda, "Id_Venda") equals venda.Id
                where (!dataInicial.HasValue || venda.DataVenda >= dataInicial.Value)
                where (!dataFinal.HasValue || venda.DataVenda <= dataFinal.Value)
                select new
                {
                    TotalItem = itemVenda.Total,
                    RevendedorId = venda.Revendedor.Id,
                    NomeFantasiaRevendedor = venda.Revendedor.NomeFantasia
                };

            // Agrupamento por Revendedor
            var listaRevendedores = itensFiltrados
                .GroupBy(item => new { item.RevendedorId, item.NomeFantasiaRevendedor })
                .Select(grupo => new
                {
                    RevendedorId = grupo.Key.RevendedorId,
                    NomeRevendedor = grupo.Key.NomeFantasiaRevendedor,
                    ValorTotalVendido = grupo.Sum(x => x.TotalItem)
                })
                .OrderByDescending(x => x.ValorTotalVendido)
                .ToList();

            // Gerar PDF
            using var ms = new MemoryStream();
            using (var writer = new iText.Kernel.Pdf.PdfWriter(ms))
            using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            using (var document = new iText.Layout.Document(pdf))
            {
                var boldFont = iText.Kernel.Font.PdfFontFactory.CreateFont(
                    iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD
                );
                var regularFont = iText.Kernel.Font.PdfFontFactory.CreateFont(
                    iText.IO.Font.Constants.StandardFonts.HELVETICA
                );

                // ======= TÍTULO ========
                var titulo = new iText.Layout.Element.Paragraph("Relatório - Revendedores Mais Ativos")
                    .SetFont(boldFont)
                    .SetFontSize(20)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE)
                    .SetBackgroundColor(new iText.Kernel.Colors.DeviceRgb(47, 58, 74))
                    .SetPadding(10)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginBottom(10);

                document.Add(titulo);

                // ======= PERÍODO ========
                string textoPeriodo =
                    $"Período: {(dataInicial.HasValue ? dataInicial.Value.ToString("dd/MM/yyyy") : "--")}  até  {(dataFinal.HasValue ? dataFinal.Value.ToString("dd/MM/yyyy") : "--")}";

                var periodo = new iText.Layout.Element.Paragraph(textoPeriodo)
                    .SetFont(boldFont)
                    .SetFontSize(12)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.BLACK)
                    .SetMarginBottom(20);

                document.Add(periodo);

                // ====== TABELA ======
                var tabela = new iText.Layout.Element.Table(new float[] { 3, 2 })
                    .UseAllAvailableWidth();

                var corCabecalho = new iText.Kernel.Colors.DeviceRgb(47, 58, 74);

                tabela.AddHeaderCell(
                    new iText.Layout.Element.Cell()
                        .Add(new iText.Layout.Element.Paragraph("Revendedor").SetFont(boldFont).SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE))
                        .SetBackgroundColor(corCabecalho)
                        .SetPadding(8)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                );

                tabela.AddHeaderCell(
                    new iText.Layout.Element.Cell()
                        .Add(new iText.Layout.Element.Paragraph("Total Vendido (R$)").SetFont(boldFont).SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE))
                        .SetBackgroundColor(corCabecalho)
                        .SetPadding(8)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                );

                foreach (var revendedor in listaRevendedores)
                {
                    tabela.AddCell(
                        new iText.Layout.Element.Cell()
                            .Add(new iText.Layout.Element.Paragraph(revendedor.NomeRevendedor).SetFont(regularFont))
                            .SetPadding(6)
                    );

                    tabela.AddCell(
                        new iText.Layout.Element.Cell()
                            .Add(new iText.Layout.Element.Paragraph(revendedor.ValorTotalVendido.ToString("N2")).SetFont(regularFont).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                            .SetPadding(6)
                    );
                }

                document.Add(tabela);

                // ====== RODAPÉ ======
                var rodape = new iText.Layout.Element.Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFont(regularFont)
                    .SetFontSize(11)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetMarginTop(25);

                document.Add(rodape);
            }

            return File(ms.ToArray(), "application/pdf", "RevendedorMaisAtivo.pdf");
        }


        public IActionResult LucroPorProduto(DateTime? dataInicial, DateTime? dataFinal)
        {
            // 1. Filtrar itens pela data da venda
            var itensFiltrados =
                from itemVenda in _context.ItensVendas
                join venda in _context.Vendas
                    on EF.Property<int>(itemVenda, "Id_Venda") equals venda.Id
                where (!dataInicial.HasValue || venda.DataVenda >= dataInicial.Value)
                where (!dataFinal.HasValue || venda.DataVenda <= dataFinal.Value)
                select itemVenda;

            // 2. Agrupar os produtos filtrados
            var produtosAgrupados = itensFiltrados
                .GroupBy(item => new
                {
                    ProdutoId = item.Produto.Id,
                    Nome = item.Produto.Nome,
                    PrecoVenda = item.Produto.Preco,
                    PrecoCompra = item.Produto.PrecoCompra
                })
                .Select(grupoProduto => new
                {
                    ProdutoId = grupoProduto.Key.ProdutoId,
                    Nome = grupoProduto.Key.Nome,
                    PrecoCompra = grupoProduto.Key.PrecoCompra,
                    PrecoVenda = grupoProduto.Key.PrecoVenda,
                    QuantidadeVendida = grupoProduto.Sum(item => item.Quantidade)
                })
                .ToList();

            // 3. Calcular lucros
            var listaLucros = produtosAgrupados
                .Select(produto => new
                {
                    produto.ProdutoId,
                    produto.Nome,
                    produto.PrecoCompra,
                    produto.PrecoVenda,
                    LucroUnidade = produto.PrecoVenda - produto.PrecoCompra,
                    produto.QuantidadeVendida,
                    LucroTotal = (produto.PrecoVenda - produto.PrecoCompra) * produto.QuantidadeVendida
                })
                .OrderByDescending(resultado => resultado.LucroTotal)
                .ToList();

            // Fontes
            var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            using var ms = new MemoryStream();
            using (var writer = new PdfWriter(ms))
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                // ======= TÍTULO =======
                var titulo = new Paragraph("Relatório - Lucro por Produto")
                    .SetFont(fontBold)
                    .SetFontSize(20)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBackgroundColor(new DeviceRgb(47, 58, 74)) // cor do sistema
                    .SetPadding(10)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginBottom(10);

                document.Add(titulo);

                // ======= PERÍODO =======
                string textoPeriodo =
                    $"Período: {(dataInicial.HasValue ? dataInicial.Value.ToString("dd/MM/yyyy") : "--")}  até  {(dataFinal.HasValue ? dataFinal.Value.ToString("dd/MM/yyyy") : "--")}";

                var periodo = new Paragraph(textoPeriodo)
                    .SetFont(fontBold)
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.BLACK)
                    .SetMarginBottom(20);

                document.Add(periodo);

                // ======= TABELA =======
                var table = new Table(5).UseAllAvailableWidth();
                var headerBackground = new DeviceRgb(47, 58, 74); // cor padrão do sistema

                // Cabeçalhos da tabela
                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Produto").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Qtd Vendida").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Preço Compra (R$)").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Preço Venda (R$)").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Lucro Total (R$)").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                // Linhas
                foreach (var item in listaLucros)
                {
                    table.AddCell(new Cell().Add(new Paragraph(item.Nome).SetFont(fontNormal)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(item.QuantidadeVendida.ToString()).SetFont(fontNormal).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(item.PrecoCompra.ToString("N2")).SetFont(fontNormal).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(item.PrecoVenda.ToString("N2")).SetFont(fontNormal).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(item.LucroTotal.ToString("N2")).SetFont(fontNormal).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)).SetPadding(6));
                }

                document.Add(table);

                // Rodapé
                var rodape = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFont(fontNormal)
                    .SetFontSize(11)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetMarginTop(25);

                document.Add(rodape);
            }

            return File(ms.ToArray(), "application/pdf", "LucroPorProduto.pdf");
        }


    }
}

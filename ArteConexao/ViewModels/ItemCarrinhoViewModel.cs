﻿namespace ArteConexao.ViewModels
{
    public class ItemCarrinhoViewModel
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid StandId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public string ImagemUrl { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorReserva { get; set; }
    }
}

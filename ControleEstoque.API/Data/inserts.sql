USE CE_DB;
GO

-- ========================================
-- FORNECEDORES
-- ========================================
INSERT INTO Fornecedores (NomeFantasia, CNPJ)
VALUES 
('Distribuidora Gama', '12345678000199'),
('Loja Tech', '98765432000188');

-- ========================================
-- PRODUTOS
-- ========================================
INSERT INTO Produtos (Nome, Preco, QauntidadeEstoque, FornecedorId)
VALUES
('Teclado Mecanico RGB', 250.00, 50, 1),
('Mouse Sem Fio', 120.50, 150, 2),
('Monitor 24 Polegadas 4k', 950.00, 20, 1);

-- ========================================
-- USUARIOS
-- ========================================
INSERT INTO Usuarios (Nome, Email, SenhaHash, Perfil, Discriminator, Turno, CPF, Setor)
VALUES
('Neymar Cliente Jr', 'menimoney@email.com', 'senha123', 0, 'Cliente', NULL, '11122233344', NULL),
('Maria do Caixa', 'maria@email.com', 'senha123', 1, 'Caixa', 'Manhã', NULL, NULL),
('Oswaldo Gerente Pai', 'oswaldo@email.com', 'senha123', 2, 'Gerente', NULL, NULL, 'Vendas Gerais');

-- ========================================
-- PEDIDOS
-- CaixaId = usuário com perfil Caixa (Id = 2)
-- ClienteId = usuário com perfil Cliente (Id = 1)
-- ========================================
INSERT INTO Pedidos (DataPedido, Status, CaixaId, ClienteId)
VALUES
(GETDATE(), 'Aberto', 2, 1),
(DATEADD(DAY, -1, GETDATE()), 'Finalizado', 2, 1),
(DATEADD(DAY, -2, GETDATE()), 'Cancelado', 2, 1);

-- ========================================
-- ITENS DO PEDIDO
-- PedidoId e ProdutoId respeitando inserts acima
-- ========================================

-- Pedido 1
INSERT INTO ItensPedido (Quantidade, PrecoUnitario, PedidoId, ProdutoId)
VALUES
(1, 250.00, 1, 1), -- Teclado
(2, 120.50, 1, 2); -- Mouse

-- Pedido 2
INSERT INTO ItensPedido (Quantidade, PrecoUnitario, PedidoId, ProdutoId)
VALUES
(1, 950.00, 2, 3); -- Monitor

-- Pedido 3
INSERT INTO ItensPedido (Quantidade, PrecoUnitario, PedidoId, ProdutoId)
VALUES
(1, 120.50, 3, 2),
(1, 250.00, 3, 1);
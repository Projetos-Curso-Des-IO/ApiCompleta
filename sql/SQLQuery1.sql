SELECT 
    'INSERT INTO Fornecedores (Id, Nome, Documento, TipoFornecedor, Ativo) VALUES (''' 
    + CAST(Id AS VARCHAR(50)) + ''', ''' 
    + Nome + ''', ''' 
    + Documento + ''', ' 
    + CAST(TipoFornecedor AS VARCHAR) + ', ' 
    + CASE WHEN Ativo = 1 THEN '1' ELSE '0' END + ');'
FROM Fornecedores

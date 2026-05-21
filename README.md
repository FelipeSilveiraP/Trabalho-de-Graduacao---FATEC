# LucroImpresso

### Trabalho de Graduação (TG) - Análise e Desenvolvimento de Sistemas (Fatec)
**Aluno:** Felipe Silveira Pessoa  
**Orientador:** [Nome do Orientador]  

---

## 📝 Descrição do Projeto
O **LucroImpresso** é um sistema web especializado em gestão de custos, controle de estoque e precificação precisa para o setor de manufatura aditiva (Impressão 3D). O objetivo principal é retirar pequenos empreendedores e *makers* do "chutômetro" financeiro, fornecendo uma ferramenta científica para cálculo de custos reais de produção (insumos, depreciação de maquinário, energia elétrica e mão de obra).

---

## 🛠️ Stack Técnica
* **Ambiente de Execução:** .NET 9
* **Framework Web:** ASP.NET Core Blazor (Componentização Reativa)
* **Persistência de Dados:** Entity Framework Core (Code-First)
* **Banco de Dados:** MySQL
* **Estilização & UI:** Bootstrap 5 / CSS3 Vanilla

---

## 📂 Estrutura do Repositório
A organização do repositório segue os padrões de governança de TI e rigor acadêmico:
* **`/docs`**: Contém todos os artefatos de engenharia de software e modelagem de negócios aprovados (BPMN, Caso de Uso, Elicitação de Requisitos, EAP, TAP, Matriz SWOT e 5W2H).
* **`/src`**: Código-fonte da aplicação desenvolvido em C#.
  * `LucroImpresso.Web/`: Projeto principal Blazor contendo as Camadas de Models, Interfaces, Repositories e Services.

---

## 🔍 Rastreabilidade de Regras de Negócio (RN)
Para fins de auditoria da banca avaliadora, as regras de negócio cruciais descritas na documentação técnica estão implementadas e mapeadas no código-fonte conforme os identificadores abaixo:

| Identificador | Descrição da Regra de Negócio | Mapeamento no Sistema |
| :--- | :--- | :--- |
| **RN001** | O preço do material é calculado estritamente por grama (`Preço do Rolo / Peso Total em Gramas`). | `Models/Material.cs` e `MaterialPage.razor` |
| **RN002** | Orçamento gerado com insumo insuficiente no banco dispara alerta visual, mas permite gravação em estado "Pendente". | `Components/Pages/Orcamentos/` |
| **RN003** | Gatilho de baixa física de estoque ocorre estritamente quando o orçamento transita para "Concluído" ou "Falha de Impressão" (Espaguete). | `Repositories/OrcamentoRepository.cs` |
| **RN004** | Exclusões manuais de registros exigem confirmação explícita em tela do usuário para prevenção de destruição de dados. | Camada de Visão Blazor (`Components/Pages/`) |
| **RN005** | Rotina automática de expurgo via *Timer* para exclusão lógica/física de registros inativos há mais de 6 meses. | `Services/DatabaseCleanupService.cs` |

---

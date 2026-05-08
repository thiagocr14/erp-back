namespace ErpAcademico.Application.DTOs;

public class RealizarVendaDto
{
    public List<ItemVendaDto> Itens { get; set; }
        = new();
}
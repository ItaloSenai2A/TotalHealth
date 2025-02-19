﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using TotalHealth.Models;

public class Usuario
{
    public Guid UsuarioId { get; set; }
    public string Nome { get; set; }
    public string? Email { get; set; }
    public string? Senha { get; set; }
    public string Telefone { get; set; }
    public string Cpf { get; set; }
    public string Genero { get; set; }
    public string TipoSanguineo { get; set; }
    public string Endereco { get; set; }

    public Guid? UserId { get; set; }
    public IdentityUser? User { get; set; }
}

using Microsoft.AspNetCore.Identity;

namespace ApiComp.Extenssions
{
	public class IdentityMensagensPortugues : IdentityErrorDescriber
	{
		public override IdentityError DefaultError()
		{
			return new IdentityError
			{
				Code = nameof(DefaultError),
				Description = "Ocorreu um erro desconhecido."
			};
		}

		public override IdentityError ConcurrencyFailure()
		{
			return new IdentityError
			{
				Code = nameof(ConcurrencyFailure),
				Description = "Falha de concorrência otimista."
			};
		}

		public override IdentityError PasswordMismatch()
		{
			return new IdentityError
			{
				Code = nameof(PasswordMismatch),
				Description = "Senha incorreta."
			};
		}

		public override IdentityError InvalidToken()
		{
			return new IdentityError
			{
				Code = nameof(InvalidToken),
				Description = "Token inválido."
			};
		}

		public override IdentityError LoginAlreadyAssociated()
		{
			return new IdentityError
			{
				Code = nameof(LoginAlreadyAssociated),
				Description = "Já existe um usuário com este login."
			};
		}

		public override IdentityError InvalidUserName(string userName)
		{
			return new IdentityError
			{
				Code = nameof(InvalidUserName),
				Description = $"Login '{userName}' é inválido."
			};
		}

		public override IdentityError InvalidEmail(string email)
		{
			return new IdentityError
			{
				Code = nameof(InvalidEmail),
				Description = $"Email '{email}' é inválido."
			};
		}

		public override IdentityError DuplicateUserName(string userName)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateUserName),
				Description = $"Login '{userName}' já está sendo utilizado."
			};
		}

		public override IdentityError DuplicateEmail(string email)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateEmail),
				Description = $"Email '{email}' já está sendo utilizado."
			};
		}

		public override IdentityError InvalidRoleName(string role)
		{
			return new IdentityError
			{
				Code = nameof(InvalidRoleName),
				Description = $"A permissão '{role}' é inválida."
			};
		}

		public override IdentityError DuplicateRoleName(string role)
		{
			return new IdentityError
			{
				Code = nameof(DuplicateRoleName),
				Description = $"A permissão '{role}' já existe."
			};
		}

		public override IdentityError UserAlreadyHasPassword()
		{
			return new IdentityError
			{
				Code = nameof(UserAlreadyHasPassword),
				Description = "Usuário já possui uma senha definida."
			};
		}

		public override IdentityError UserLockoutNotEnabled()
		{
			return new IdentityError
			{
				Code = nameof(UserLockoutNotEnabled),
				Description = "Lockout não está habilitado para este usuário."
			};
		}

		public override IdentityError UserAlreadyInRole(string role)
		{
			return new IdentityError
			{
				Code = nameof(UserAlreadyInRole),
				Description = $"Usuário já possui a permissão '{role}'."
			};
		}

		public override IdentityError UserNotInRole(string role)
		{
			return new IdentityError
			{
				Code = nameof(UserNotInRole),
				Description = $"Usuário não tem a permissão '{role}'."
			};
		}

		public override IdentityError PasswordTooShort(int length)
		{
			return new IdentityError
			{
				Code = nameof(PasswordTooShort),
				Description = $"Senhas devem conter pelo menos {length} caracteres."
			};
		}

		// Você pode adicionar outros métodos conforme necessário
		public override IdentityError PasswordRequiresDigit()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresDigit),
				Description = "A senha deve conter pelo menos um dígito ('0'-'9')."
			};
		}

		public override IdentityError PasswordRequiresLower()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresLower),
				Description = "A senha deve conter pelo menos uma letra minúscula ('a'-'z')."
			};
		}

		public override IdentityError PasswordRequiresUpper()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresUpper),
				Description = "A senha deve conter pelo menos uma letra maiúscula ('A'-'Z')."
			};
		}

		public override IdentityError PasswordRequiresNonAlphanumeric()
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresNonAlphanumeric),
				Description = "A senha deve conter pelo menos um caractere não alfanumérico."
			};
		}

		public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
		{
			return new IdentityError
			{
				Code = nameof(PasswordRequiresUniqueChars),
				Description = $"A senha deve conter pelo menos {uniqueChars} caracteres distintos."
			};
		}
	}
}
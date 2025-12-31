// bflat minimal runtime library
// Copyright (C) 2021-2022 Michal Strehovsky
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace System.Reflection
{
	public sealed class DefaultMemberAttribute : Attribute
	{
		public DefaultMemberAttribute(string memberName) { }
	}

	public sealed class AssemblyCompanyAttribute : Attribute
	{
		public AssemblyCompanyAttribute(string company) { }
	}

	public sealed class AssemblyConfigurationAttribute : Attribute
	{
		public AssemblyConfigurationAttribute(string configuration)
		{
		}
	}

	public sealed class AssemblyFileVersionAttribute : Attribute
	{
		public AssemblyFileVersionAttribute(string version)
		{
		}
	}

	public sealed class AssemblyInformationalVersionAttribute : Attribute
	{
		public AssemblyInformationalVersionAttribute(string informationalVersion)
		{
		}

	}

	public sealed class AssemblyProductAttribute : Attribute
	{
		public AssemblyProductAttribute(string product)
		{
		}

	}

	public sealed class AssemblyTitleAttribute : Attribute
	{
		public AssemblyTitleAttribute(string title)
		{
		}
	}

	public sealed class AssemblyVersionAttribute : Attribute
	{
		public AssemblyVersionAttribute(string version)
		{
		}
	}
}

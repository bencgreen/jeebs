﻿namespace Jeebs.LinkTests
{
	public interface ILink_Map
	{
		void IOk_Input_Maps_To_Next_Type();
		void IOk_Input_When_IError_Returns_IError();
		void IOk_Input_When_IOk_Catches_Exception();
		void IOk_ValueType_Input_When_IError_Returns_IError();
		void IOk_ValueType_Input_When_IOk_Catches_Exception();
		void IOk_ValueType_Input_When_IOk_Maps_To_Next_Type();
		void IOk_Value_Input_When_IError_Returns_IError();
		void IOk_Value_Input_When_IOk_Catches_Exception();
		void IOk_Value_Input_When_IOk_Maps_To_Next_Type();
	}
}
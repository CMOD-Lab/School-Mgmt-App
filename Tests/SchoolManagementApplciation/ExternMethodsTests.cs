using System;
using System.Globalization;
using Xunit;

namespace SchoolManagementApplciation.Tests
{
    /// <summary>
    /// Unit tests for ExternMethods extension class (Utils.cs).
    /// Tests cover IsNumber, ToMonthName, ToShortMonthName extension methods.
    /// </summary>
    public class ExternMethodsTests
    {
        // ─────────────────────────────────────────────────────────────────────
        // IsNumber – numeric types should return their Type
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void IsNumber_SByte_ReturnsSByteType()
        {
            object value = (sbyte)1;
            Assert.Equal(typeof(sbyte), value.IsNumber());
        }

        [Fact]
        public void IsNumber_Byte_ReturnsByteType()
        {
            object value = (byte)255;
            Assert.Equal(typeof(byte), value.IsNumber());
        }

        [Fact]
        public void IsNumber_Short_ReturnsShortType()
        {
            object value = (short)100;
            Assert.Equal(typeof(short), value.IsNumber());
        }

        [Fact]
        public void IsNumber_UShort_ReturnsUShortType()
        {
            object value = (ushort)200;
            Assert.Equal(typeof(ushort), value.IsNumber());
        }

        [Fact]
        public void IsNumber_Int_ReturnsIntType()
        {
            object value = 42;
            Assert.Equal(typeof(int), value.IsNumber());
        }

        [Fact]
        public void IsNumber_UInt_ReturnsUIntType()
        {
            object value = (uint)42;
            Assert.Equal(typeof(uint), value.IsNumber());
        }

        [Fact]
        public void IsNumber_Long_ReturnsLongType()
        {
            object value = (long)1234567890123L;
            Assert.Equal(typeof(long), value.IsNumber());
        }

        [Fact]
        public void IsNumber_ULong_ReturnsULongType()
        {
            object value = (ulong)9999999999UL;
            Assert.Equal(typeof(ulong), value.IsNumber());
        }

        [Fact]
        public void IsNumber_Float_ReturnsFloatType()
        {
            object value = 3.14f;
            Assert.Equal(typeof(float), value.IsNumber());
        }

        [Fact]
        public void IsNumber_Double_ReturnsDoubleType()
        {
            object value = 3.14;
            Assert.Equal(typeof(double), value.IsNumber());
        }

        [Fact]
        public void IsNumber_Decimal_ReturnsDecimalType()
        {
            object value = 99.99m;
            Assert.Equal(typeof(decimal), value.IsNumber());
        }

        // ─────────────────────────────────────────────────────────────────────
        // IsNumber – non-numeric types should return null
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void IsNumber_String_ReturnsNull()
        {
            object value = "hello";
            Assert.Null(value.IsNumber());
        }

        [Fact]
        public void IsNumber_Bool_ReturnsNull()
        {
            object value = true;
            Assert.Null(value.IsNumber());
        }

        [Fact]
        public void IsNumber_DateTime_ReturnsNull()
        {
            object value = DateTime.Now;
            Assert.Null(value.IsNumber());
        }

        [Fact]
        public void IsNumber_Object_ReturnsNull()
        {
            object value = new object();
            Assert.Null(value.IsNumber());
        }

        [Fact]
        public void IsNumber_Char_ReturnsNull()
        {
            object value = 'A';
            Assert.Null(value.IsNumber());
        }

        [Fact]
        public void IsNumber_AnonymousObject_ReturnsNull()
        {
            object value = new { X = 1 };
            Assert.Null(value.IsNumber());
        }

        // ─────────────────────────────────────────────────────────────────────
        // IsNumber – boundary / edge cases
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void IsNumber_IntZero_ReturnsIntType()
        {
            object value = 0;
            Assert.Equal(typeof(int), value.IsNumber());
        }

        [Fact]
        public void IsNumber_IntMaxValue_ReturnsIntType()
        {
            object value = int.MaxValue;
            Assert.Equal(typeof(int), value.IsNumber());
        }

        [Fact]
        public void IsNumber_IntMinValue_ReturnsIntType()
        {
            object value = int.MinValue;
            Assert.Equal(typeof(int), value.IsNumber());
        }

        [Fact]
        public void IsNumber_DoubleNaN_ReturnsDoubleType()
        {
            object value = double.NaN;
            Assert.Equal(typeof(double), value.IsNumber());
        }

        [Fact]
        public void IsNumber_DoublePositiveInfinity_ReturnsDoubleType()
        {
            object value = double.PositiveInfinity;
            Assert.Equal(typeof(double), value.IsNumber());
        }

        [Fact]
        public void IsNumber_DoubleNegativeInfinity_ReturnsDoubleType()
        {
            object value = double.NegativeInfinity;
            Assert.Equal(typeof(double), value.IsNumber());
        }

        [Fact]
        public void IsNumber_DecimalZero_ReturnsDecimalType()
        {
            object value = 0m;
            Assert.Equal(typeof(decimal), value.IsNumber());
        }

        [Fact]
        public void IsNumber_DecimalMaxValue_ReturnsDecimalType()
        {
            object value = decimal.MaxValue;
            Assert.Equal(typeof(decimal), value.IsNumber());
        }

        [Fact]
        public void IsNumber_LongMaxValue_ReturnsLongType()
        {
            object value = long.MaxValue;
            Assert.Equal(typeof(long), value.IsNumber());
        }

        [Fact]
        public void IsNumber_ByteZero_ReturnsByteType()
        {
            object value = (byte)0;
            Assert.Equal(typeof(byte), value.IsNumber());
        }

        [Fact]
        public void IsNumber_ByteMaxValue_ReturnsByteType()
        {
            object value = byte.MaxValue;
            Assert.Equal(typeof(byte), value.IsNumber());
        }

        [Fact]
        public void IsNumber_FloatZero_ReturnsFloatType()
        {
            object value = 0.0f;
            Assert.Equal(typeof(float), value.IsNumber());
        }

        [Fact]
        public void IsNumber_NegativeInt_ReturnsIntType()
        {
            object value = -100;
            Assert.Equal(typeof(int), value.IsNumber());
        }

        [Fact]
        public void IsNumber_NegativeDouble_ReturnsDoubleType()
        {
            object value = -3.14;
            Assert.Equal(typeof(double), value.IsNumber());
        }

        // ─────────────────────────────────────────────────────────────────────
        // ToMonthName
        // ─────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        public void ToMonthName_AllMonths_ReturnsNonEmptyString(int month)
        {
            var dt = new DateTime(2024, month, 1);
            string result = dt.ToMonthName();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ToMonthName_January_MatchesCultureInfo()
        {
            var dt = new DateTime(2024, 1, 15);
            string expected = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1);
            Assert.Equal(expected, dt.ToMonthName());
        }

        [Fact]
        public void ToMonthName_December_MatchesCultureInfo()
        {
            var dt = new DateTime(2024, 12, 31);
            string expected = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(12);
            Assert.Equal(expected, dt.ToMonthName());
        }

        [Fact]
        public void ToMonthName_June_MatchesCultureInfo()
        {
            var dt = new DateTime(2024, 6, 1);
            string expected = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(6);
            Assert.Equal(expected, dt.ToMonthName());
        }

        [Fact]
        public void ToMonthName_DifferentDaysInSameMonth_ReturnSameName()
        {
            var dt1 = new DateTime(2024, 5, 1);
            var dt2 = new DateTime(2024, 5, 31);
            Assert.Equal(dt1.ToMonthName(), dt2.ToMonthName());
        }

        [Fact]
        public void ToMonthName_DifferentYearsSameMonth_ReturnSameName()
        {
            var dt1 = new DateTime(2020, 3, 15);
            var dt2 = new DateTime(2024, 3, 15);
            Assert.Equal(dt1.ToMonthName(), dt2.ToMonthName());
        }

        // ─────────────────────────────────────────────────────────────────────
        // ToShortMonthName
        // ─────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        public void ToShortMonthName_AllMonths_ReturnsNonEmptyString(int month)
        {
            var dt = new DateTime(2024, month, 1);
            string result = dt.ToShortMonthName();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ToShortMonthName_January_MatchesCultureInfo()
        {
            var dt = new DateTime(2024, 1, 15);
            string expected = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(1);
            Assert.Equal(expected, dt.ToShortMonthName());
        }

        [Fact]
        public void ToShortMonthName_December_MatchesCultureInfo()
        {
            var dt = new DateTime(2024, 12, 31);
            string expected = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(12);
            Assert.Equal(expected, dt.ToShortMonthName());
        }

        [Fact]
        public void ToShortMonthName_IsShorterThanOrEqualToFullName()
        {
            var dt = new DateTime(2024, 3, 1);
            string full = dt.ToMonthName();
            string abbr = dt.ToShortMonthName();
            Assert.True(abbr.Length <= full.Length);
        }

        [Fact]
        public void ToShortMonthName_DifferentDaysInSameMonth_ReturnSameName()
        {
            var dt1 = new DateTime(2024, 11, 1);
            var dt2 = new DateTime(2024, 11, 30);
            Assert.Equal(dt1.ToShortMonthName(), dt2.ToShortMonthName());
        }

        // ─────────────────────────────────────────────────────────────────────
        // ToMonthName / ToShortMonthName – cross-method consistency
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void ToMonthName_AndToShortMonthName_SameMonth_AreConsistent()
        {
            var dt = new DateTime(2024, 8, 20);
            string full = dt.ToMonthName();
            string abbr = dt.ToShortMonthName();

            Assert.NotEmpty(full);
            Assert.NotEmpty(abbr);
        }

        [Fact]
        public void ToMonthName_ReturnsStringNotContainingDigits()
        {
            var dt = new DateTime(2024, 7, 4);
            string result = dt.ToMonthName();
            // Month names should not contain digits
            foreach (char c in result)
                Assert.False(char.IsDigit(c), $"Month name '{result}' should not contain digits");
        }

        [Fact]
        public void ToShortMonthName_ReturnsStringNotContainingDigits()
        {
            var dt = new DateTime(2024, 9, 1);
            string result = dt.ToShortMonthName();
            foreach (char c in result)
                Assert.False(char.IsDigit(c), $"Short month name '{result}' should not contain digits");
        }

        [Fact]
        public void ToMonthName_AllTwelveMonths_AreDistinct()
        {
            var names = new System.Collections.Generic.HashSet<string>();
            for (int m = 1; m <= 12; m++)
            {
                var dt = new DateTime(2024, m, 1);
                names.Add(dt.ToMonthName());
            }
            Assert.Equal(12, names.Count);
        }

        [Fact]
        public void ToShortMonthName_AllTwelveMonths_AreDistinct()
        {
            var names = new System.Collections.Generic.HashSet<string>();
            for (int m = 1; m <= 12; m++)
            {
                var dt = new DateTime(2024, m, 1);
                names.Add(dt.ToShortMonthName());
            }
            Assert.Equal(12, names.Count);
        }
    }
}

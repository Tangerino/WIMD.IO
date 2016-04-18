using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Formulas;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class FormulasTests : BaseTests
    {
        [TestMethod()]
        public void Formula_CRUD_Positive()
        {
            // create
            Formula formula = null;
            Func<Task> asyncFunction = async () => formula = await CreateFormula(Client);
            asyncFunction.ShouldNotThrow();
            formula.Should().NotBeNull();

            // read
            asyncFunction = async () => formula = await Client.ReadFormula(formula.Id);
            asyncFunction.ShouldNotThrow();
            formula.Should().NotBeNull();

            // read list
            IEnumerable<Formula> formulas = null;
            asyncFunction = async () => formulas = await Client.ReadFormulas();
            asyncFunction.ShouldNotThrow();
            formulas.Should().NotBeNullOrEmpty();
            formulas.Any(x => x.Id == formula.Id).Should().BeTrue();

            // update
            var update = new UpdateFormula(formula)
            {
                Name = formula.Name + " Updated",
                Code = formula.Code + " * 1",
            };
            asyncFunction = async () => formula = await Client.UpdateFormula(formula.Id, update);
            asyncFunction.ShouldNotThrow();
            formula.Should().NotBeNull();
            formula.Name.Should().Be(update.Name);
            formula.Code.Should().Be(update.Code);
            formula.Library.Should().Be(update.Library);

            // delete
            asyncFunction = async () => await Client.DeleteFormula(formula.Id);
            asyncFunction.ShouldNotThrow();

            // read list
            asyncFunction = async () => formulas = await Client.ReadFormulas();
            asyncFunction.ShouldNotThrow();
            formulas.Any(x => x.Id == formula.Id).Should().BeFalse();
        }
    }
}

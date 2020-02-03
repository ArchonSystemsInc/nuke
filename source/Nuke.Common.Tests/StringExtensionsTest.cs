﻿// Copyright 2019 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using FluentAssertions;
using Nuke.Common.Utilities;
using Xunit;

namespace Nuke.Common.Tests
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("a b c", "\"a b c\"")]
        [InlineData("\"a b c\"", "\"a b c\"")]
        public void TestDoubleQuoteIfNeeded(string input, string output)
        {
            input.DoubleQuoteIfNeeded().Should().Be(output);
        }

        [Theory]
        [InlineData("a\nb\nc")]
        [InlineData("a\r\nb\r\nc")]
        public void TestSplitLineBreaks(string input)
        {
            input.SplitLineBreaks().Should().BeEquivalentTo("a", "b", "c");
        }

        [Theory]
        [InlineData("arg", '\"', "arg")]
        [InlineData("\"arg\"", '\"', "arg")]
        [InlineData("\"\"arg\"\"", '\"', "\"arg\"")]
        [InlineData("\"arg", '\"', "\"arg")]
        [InlineData("arg\"", '\"', "arg\"")]
        public void TestTrimMatchingQuotes(string input, char quote, string expected)
        {
            input.TrimMatchingQuotes(quote).Should().Be(expected);
        }

        [Fact]
        public void TestSplit()
        {
            "msbuild-configuration".Split((x, i) => !char.IsLetter(x))
                .Should().Equal("msbuild", "configuration");

            "MSBuildConfiguration".SplitCamelHumps()
                .Should().Equal("MSBuild", "Configuration");

            "NuGetGitHubKey".SplitCamelHumps("NuGet", "GitHub")
                .Should().Equal("NuGet", "GitHub", "Key");

            "NuGetKeyGitHubKey".SplitCamelHumps("NuGet", "GitHub")
                .Should().Equal("NuGet", "Key", "GitHub", "Key");
        }

        [Theory]
        [InlineData("dep.test.env.=ExitCode=000000", @"[^\.]=", 22, "dep.test.env.=ExitCode", "000000")]
        public void TestIndexOfRegex(string input, string expression, int expectedIndex, string expectedKey, string expectedValue)
        {
            var index = input.IndexOfRegex(expression);
            index.Should().Be(expectedIndex);

            input.Substring(0, index).Should().Be(expectedKey);
            input.Substring(index + 1).Should().Be(expectedValue);
        }
    }
}

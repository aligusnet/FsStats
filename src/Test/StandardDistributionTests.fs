module ``Standard Normal distribution tests``

open Xunit
open FsUnit.Xunit

open FsStats

[<Fact>]
let ``Normal distribution`` () =
    let rnd = new System.Random()
    StandardDistribution.mean |> should (equalWithin 1e-5) 0.0
    StandardDistribution.variance |> should (equalWithin 1e-5) 1.0
    StandardDistribution.stddev |> should (equalWithin 1e-5) 1.0
    StandardDistribution.cdf 0.0 |> should (equalWithin 1e-5) 0.5
    StandardDistribution.cdf 100.0 |> should (equalWithin 1e-5) 1.0
    StandardDistribution.cdf -1.0 |> should (equalWithin 1e-4) (0.5 - 0.6827*0.5)
    StandardDistribution.cdf 1.0 |> should (equalWithin 1e-4) (0.5 + 0.6827*0.5)
    StandardDistribution.random rnd |> should be ofExactType<float>
    StandardDistribution.sample rnd 19 |> should haveLength 19

[<Fact>]
let ``Standard Normal distribution confidence intervals`` () =
    let interval x = (StandardDistribution.cdf x) - (StandardDistribution.cdf -x)
    interval 1.0 |> should (equalWithin 1e-4) 0.6827
    interval 1.96 |> should (equalWithin 1e-4) 0.95
    interval 2.0 |> should (equalWithin 1e-4) 0.9545
    interval 3.0 |> should (equalWithin 1e-4) 0.9973

[<Fact>]
let ``Mean of generated random values should be close to the distribution's mean and stddev`` () =
    let rnd = new System.Random()
    let eps = 0.1
    let summary = SummaryStatistics.create (StandardDistribution.sample rnd 1001)
    summary.Mean |> should (equalWithin eps) 0.0
    summary.StdDev |> should (equalWithin eps) 1.0


[<Theory>]
[<InlineData(0.0, 0.39894228)>]
[<InlineData(0.5, 0.35206533)>]
[<InlineData(-0.5, 0.35206533)>]
[<InlineData(-10.0, 0.0)>]
let ``Probability density function`` (x, pdf) =
    StandardDistribution.pdf x |> should (equalWithin 1e-7) pdf


[<Theory>]
[<InlineData(0.5, 0.0)>]
[<InlineData(0.15865, -1.0)>]
[<InlineData(0.84135, 1.0)>]
let ``Quanitile if inverse of CDF`` (p, x) = 
    StandardDistribution.quantile p |> should (equalWithin 2e-4) x
    StandardDistribution.cdf x |> should (equalWithin 1e-4) p

module ``Normal distribution tests``

open Xunit
open FsUnit.Xunit

open FsStats

[<Fact>]
let ``Normal distribution`` () =
    let rnd = new System.Random()
    let mu, sigma = 10.0, 2.0
    let nd = NormalDistribution.create mu sigma
    let cdf = NormalDistribution.cdf nd
    NormalDistribution.mean nd |> should (equalWithin 1e-5) mu
    NormalDistribution.variance nd |> should (equalWithin 1e-5) (sigma*sigma)
    NormalDistribution.stddev nd |> should (equalWithin 1e-5) sigma
    cdf mu |> should (equalWithin 1e-5) 0.5
    cdf 100.0 |> should (equalWithin 1e-5) 1.0
    cdf (mu - sigma) |> should (equalWithin 1e-4) (0.5 - 0.6827*0.5)
    cdf (mu + sigma) |> should (equalWithin 1e-4) (0.5 + 0.6827*0.5)
    NormalDistribution.random nd rnd |> should be ofExactType<float>
    NormalDistribution.sample nd rnd 20 |> should haveLength 20

[<Theory>]
[<InlineData(0.0, 1.0)>]
[<InlineData(10.0, 2.0)>]
[<InlineData(-5.0, 3.0)>]
let ``Normal distribution confidence intervals`` (mu, sigma) =
    let nd = NormalDistribution.create mu sigma
    let cdf = NormalDistribution.cdf nd
    let interval x = (cdf (mu + x)) - (cdf (mu - x))
    interval sigma |> should (equalWithin 1e-4) 0.6827
    interval (1.96*sigma) |> should (equalWithin 1e-4) 0.95
    interval (2.0*sigma) |> should (equalWithin 1e-4) 0.9545
    interval (3.0*sigma) |> should (equalWithin 1e-4) 0.9973

[<Fact>]
let ``Mean of generated random values should be close to the distribution's mean and stddev`` () =
    let rnd = new System.Random()
    let mu, sigma = 11.0, 3.0
    let eps = 0.1 * sigma
    let nd = NormalDistribution.create mu sigma
    let summary = SummaryStatistics.create (NormalDistribution.sample nd rnd 1000)
    summary.Mean |> should (equalWithin eps) mu
    summary.StdDev |> should (equalWithin eps) sigma

[<Theory>]
[<InlineData(0.0, 1.0, 2.0, 2.0)>]
[<InlineData(10.0, 3.0, 1.0, -3.0)>]
let ``z-score tests`` (mu, sigma, x, expectedZScore) =
    let nd = NormalDistribution.create mu sigma
    NormalDistribution.zScore nd x |> should (equalWithin 1e-5) expectedZScore

[<Theory>]
[<InlineData(11.0, 3.0, 11.0, 0.13298076)>]
[<InlineData(-15.0, 7.0, 5.0, 0.00096201)>]
[<InlineData(-15.0, 7.0, -35.0, 0.00096201)>]
[<InlineData(-15.0, 7.0, -5.0, 0.02054255)>]
[<InlineData(12.0, 3.0, -100, 0.0)>]
let ``Probability density function`` (mu, sigma, x, pdf) =
    let nd = NormalDistribution.create mu sigma
    NormalDistribution.pdf nd x |> should (equalWithin 1e-7) pdf

[<Theory>]
[<InlineData(10.0, 2.0)>]
[<InlineData(-5.0, 3.0)>]
let ``Quanitile if inverse of CDF`` (mu, sigma) =
    let nd = NormalDistribution.create mu sigma
    let cdf = NormalDistribution.cdf nd
    let quantile = NormalDistribution.quantile nd
    quantile (0.5 + 0.6827*0.5) |> should (equalWithin 1e-3) (mu + sigma)
    cdf (mu + sigma) |> should (equalWithin 1e-4) (0.5 + 0.6827*0.5)
    quantile (0.5 - 0.6827*0.5) |> should (equalWithin 1e-3) (mu - sigma)
    cdf (mu - sigma) |> should (equalWithin 1e-4) (0.5 - 0.6827*0.5)

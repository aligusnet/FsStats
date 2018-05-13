module ``Summary Statistics tests``

open Xunit
open FsUnit.Xunit

open FsStats.Summary

[<Fact>]
let ``Summary Statistics`` () = 
    // Generated by Normal.Distribution(10.0, 2.0).Samples 30
    let data = [|13.7858911526618; 9.55203558961586; 8.73590083796644; 14.6446895575045; 7.61758207758193; 
                 7.81767470271187; 9.1694740424746; 11.6114177177951; 10.2582801800003; 8.540936510641; 
                 7.98765220604253; 11.9092751627251; 6.04344060258961; 7.64167613512391; 10.5588747466957; 
                 9.7008682321666; 11.3431376347234; 9.21564519814689; 9.911795373412; 10.1239694706768; 
                 10.7603987140471; 10.4357884213866; 10.3055137320688; 12.2362093650794; 8.77565517588214; 
                 6.43386624817808; 12.0007153989415; 12.8771963800697; 9.24429657175203; 10.3720301023293|]
    let stats = new SummaryStatistics(data)
    stats.Mean |> should (equalWithin 1e-5)  9.98706
    stats.Variance |> should (equalWithin 1e-5) 4.08428
    stats.StdDev |> should (equalWithin 1e-5) 2.02096
    stats.zScore 16.0 |> should (equalWithin 1e-5) 2.97528


[<Theory>]
[<InlineData(0.0, 1.0, 2.0, 2.0)>]
[<InlineData(10.0, 3.0, 1.0, -3.0)>]
let ``z-score tests`` (mu, sigma, x, expectedZScore) = 
    zScore mu sigma x |> should (equalWithin 1e-5) expectedZScore

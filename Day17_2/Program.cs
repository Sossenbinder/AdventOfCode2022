// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Day17_2;

//new Solver(true).Solve();
BenchmarkRunner.Run<Solver>();
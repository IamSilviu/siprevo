BnfToDfa.exe http.bnf http.mrk req
DfaToCSharp.exe http.xml Http.Message HttpMessageReader

move Http.Message.dfa ..\
move HttpMessageReader.cs ..\
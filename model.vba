Sub Scenario_TEST()
    ' Chama a função Optimizer_ON para otimizar o desempenho do Excel
    Call Optimizer_ON
    
    ' Limpa o modo de cópia do Excel
    Application.CutCopyMode = False
    
    ' Cria um resumo dos cenários na planilha ativa
    ActiveSheet.Scenarios.CreateSummary ReportType:=xlStandardSummary, _
        ResultCells:=Range("Scenario_Result")
    
    ' Copia a planilha "Scenario Summary 2"
    Sheets("Scenario Summary 2").Copy
    
    ' Define o caminho do arquivo como o mesmo local da planilha atual
    Dim filePath As String
    filePath = ThisWorkbook.Path & "\Resultados.xlsx"
    
    ' Salva o workbook atual com o nome e formato especificados
    ActiveWorkbook.SaveAs Filename:=filePath, FileFormat:=xlOpenXMLWorkbook, CreateBackup:=False
    
    ' Fecha a janela ativa
    ActiveWindow.Close
    
    ' Deleta a planilha "Scenario Summary 2"
    Sheets("Scenario Summary 2").Delete
    
    ' Chama a função Optimizer_OFF para reverter as otimizações feitas pela função Optimizer_ON
    Call Optimizer_OFF
End Sub
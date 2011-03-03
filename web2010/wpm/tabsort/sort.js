	//----------------------------------------------------------------------------------------------------------
	function TableData(noOfRows, noOfColumns, noOfHeaderRows){
		this.tableRef = null;
		this.noOfRows = null;
		this.noOfColumns = null;
		this.noOfHeaderRows = null;
		this.columnSortStatus = null;
	}
	TableData.prototype.setData = function (tableId, noOfHeaderRows) {
		this.tableRef = document.getElementById(tableId);
		this.noOfRows = this.tableRef.getElementsByTagName('tr').length - noOfHeaderRows;
		this.noOfColumns =  this.tableRef.getElementsByTagName('tr')[0].getElementsByTagName('td').length;
		this.noOfHeaderRows = noOfHeaderRows;
		this.columnSortStatus = new Array(this.noOfColumns);
		for(var i=0; i<this.columnSortStatus.length; i++){
			this.columnSortStatus[i] = 'desc';
		}
		this.tableDataArray = new Array(this.noOfRows);
		for(var i=0; i<this.noOfRows; i++){
			this.tableDataArray[i] = new Array();
			for(var j=0; j<this.noOfColumns; j++){
				this.tableDataArray[i].push(this.tableRef.getElementsByTagName('tr')[i+this.noOfHeaderRows].getElementsByTagName('td')[j].innerText);
			}
		}
	}
	TableData.prototype.sort = function (columnId, dataType) {
		if(tableData.columnSortStatus[columnId] == 'desc'){
			//sort in asc order
			for(var j=0; j<tableData.tableDataArray.length-1; j++){
				for(var i=0; i<tableData.tableDataArray.length; i++){
					if(dataType == 'int'){
						if( (i+1<tableData.tableDataArray.length) && (parseInt(tableData.tableDataArray[i][columnId]) > parseInt(tableData.tableDataArray[i+1][columnId])) ){
							//interchange column values of the 2 rows
							interchange(i);
						}
					}else if(dataType == 'string'){
						if( (i+1<tableData.tableDataArray.length) && (tableData.tableDataArray[i][columnId] > tableData.tableDataArray[i+1][columnId]) ){
							//interchange column values of the 2 rows
							interchange(i);
						}
					}
				}
			}
			tableData.columnSortStatus[columnId] = 'asc'
		}else if(tableData.columnSortStatus[columnId] == 'asc'){
			//sort in desc order
			for(var j=0; j<tableData.tableDataArray.length-1; j++){
				for(var i=0; i<tableData.tableDataArray.length; i++){
					if(dataType == 'int'){
						if( (i+1<tableData.tableDataArray.length) && (parseInt(tableData.tableDataArray[i][columnId]) < parseInt(tableData.tableDataArray[i+1][columnId])) ){
							//interchange column values of the 2 rows
							interchange(i);
						}
					}else if(dataType == 'string'){
						if( (i+1<tableData.tableDataArray.length) && (tableData.tableDataArray[i][columnId] < tableData.tableDataArray[i+1][columnId]) ){
							//interchange column values of the 2 rows
							interchange(i);
						}
					}
				}
			}
			tableData.columnSortStatus[columnId] = 'desc'
		}
		//Display the sorted data
		for(var j=0;j<tableData.noOfRows;j++){
			for(var i=0;i<tableData.noOfColumns;i++){
				tableData.tableRef.getElementsByTagName('tr')[j+tableData.noOfHeaderRows].getElementsByTagName('td')[i].innerText = tableData.tableDataArray[j][i];
			}
		}
	}
	var tableData = new TableData();
	//----------------------------------------------------------------------------------------------------------
	function interchange(i){
	var tempStr = '';
	for(var col=0; col<tableData.noOfColumns; col++){
		tempStr = tableData.tableDataArray[i][col];
		tableData.tableDataArray[i][col] = tableData.tableDataArray[i+1][col];
		tableData.tableDataArray[i+1][col] = tempStr;
	}
}
# DataFileFormatter

File formatter project for json, xml (for windows)

## Usage
```cmd
DataFileFormatter.exe [file type] [format style] [output file path] [indent character] [indent spaces count] [character encoding] [input file path]
```

The order of the switches are free. \
`input file path` or redirect from standard input is required.

example: 
```cmd
type C:\Users\user012\Desktop\inputFile.xml | DataFileFormatter.exe --xml --unformat --outfile C:\Users\user012\Desktop\afterFormatFile.xml --charset utf8
DataFileFormatter.exe --format --space --indentSpacesCount 8 C:\Users\user012\Desktop\inputFile.json
```

## switches
All the switches requires `--` prefix like `--json`.

`file type`: \
Target file type to handle \
available types: 
- json (default)
- xml

`format style`: \
Specifies the operation to do: format (add indent to make the file human friendly style) or unformat (minimize data size) \
available types: 
- format (default)
- unformat

`output file path`: \
If this switch is set, this application will output the file to the specified file path. \
You have to specify file path after `--outfile` switch. \
If not, you can get process result from standard output. \

`indent character`: \
Specifies which character to use for indentation when format style is `format`. \
Ignored if format style is `unformat`. \
available types: 
- space (default)
- tab

`indent spaces count`: \
Specifies spaces count per indentation. \
You have to specify actual spaces count value after `--indentSpacesCount` switch. \
Ignored if indent character is `tab`.
Default: 4

`character encoding`: \
Specifies character encoding to use to read or output file. \
You have to specify actual encoding name like `utf8`, `shift-jis` after `--charset` switch. \
Available values depends on what character set is available in the execution environment.
Default: UTF-8

`input file path`: \
Specifies input file to read. \
If not specified, this application will try to read from standard input. \
Actually this is not a switch. Just specify only file path. \

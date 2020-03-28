java-embedded:
	@rm -rf tmp/* northwind-mixed.jar
	@mkdir tmp/gen/ tmp/cls/
	java -jar bin/cellc-java.jar -d projects/embedded.txt tmp/gen/
# 	java -jar ../../java/bin/cellc-java.jar -d projects/embedded.txt tmp/gen/
	javac -g -d tmp/cls/ tmp/gen/*.java src/*.java
	jar cfe northwind-mixed.jar Tester -C tmp/cls/ .

csharp-embedded:
	@rm -rf dotnet/ tmp/
	@mkdir dotnet/ tmp/
	@cp projects/embedded.csproj dotnet/northwind.csproj
	bin/cellc-cs -d -g gen-list.txt projects/embedded.txt tmp/
# 	../../csharp/bin/cellc-cs -d -g gen-list.txt projects/embedded.txt tmp/
	mv tmp/generated.cs tmp/runtime.cs tmp/automata.cs tmp/typedefs.cs dotnet/
	dotnet build -c Debug dotnet/

csharp-standalone: cell/main.cell cell/northwind.cell cell/csv.cell
	@rm -rf dotnet/ tmp/
	@mkdir dotnet/ tmp/
	@cp projects/standalone.csproj dotnet/northwind.csproj
	bin/cellc-cs -d projects/standalone.txt tmp/
# 	../../csharp/bin/cellc-cs -d projects/standalone.txt tmp/
	mv tmp/generated.cs tmp/runtime.cs dotnet/
	dotnet build -c Debug dotnet/

java-standalone: cell/main.cell cell/northwind.cell cell/csv.cell
	@rm -rf northwind.jar tmp/gen/ tmp/net/
	@mkdir -p tmp/gen/
	java -jar bin/cellc-java.jar -d projects/standalone.txt tmp/gen/
# 	java -jar ../../java/bin/cellcd-java.jar -d projects/standalone.txt tmp/gen/
	javac -g -d tmp/ tmp/gen/*.java
	jar cfe northwind.jar net.cell_lang.Generated -C tmp net/

clean:
	@rm -rf tmp/* northwind.jar northwind-mixed.jar dotnet debug/*

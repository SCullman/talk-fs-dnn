# Install template
dotnet new -i Fable.Template.Elmish.React 
# Create  project
dotnet new fable-elmish-react -n awesome -lang f# -y
cd awesome
# Install npm dependencies
yarn install
cd src
# Install dotnet dependencies
dotnet restore
# Start Fable server and Webpack dev server
dotnet fable yarn-start
# In your browser, open: http://localhost:8080/
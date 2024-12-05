# OpenWeatherMAUI
Atividade avaliativa referente a disciplina de PAM (Programação Mobile)

**OpenWeatherMAUI** é um aplicativo multiplataforma desenvolvido em .NET MAUI que fornece informações detalhadas sobre o clima, utilizando a API do OpenWeather. O aplicativo exibe a previsão do clima para os próximos 5 dias, com detalhes a cada 3 horas, além das condições climáticas atuais. No dispositivo móvel, o aplicativo utiliza a localização via GPS para oferecer previsões baseadas na posição do usuário.

## Funcionalidades

- **Previsão do Clima de 5 Dias**
  - Exibe previsões detalhadas para os próximos 5 dias, com informações a cada 3 horas.
  - Mostra as condições climáticas ao longo do dia (ex: temperatura máxima, mínima, descrição do tempo).

- **Clima Atual**
  - Mostra as condições climáticas em tempo real, incluindo temperatura máxima, mínima e descrição do tempo.

- **Localização via GPS**
  - Utiliza o GPS do dispositivo para obter a localização atual do usuário e exibir as previsões do clima baseadas nesta localização.

- **Interface Intuitiva**
  - A interface do usuário exibe os dados climáticos de maneira clara e acessível, com ícones representando as condições climáticas, facilitando a compreensão.

## Tecnologias Utilizadas

- **.NET MAUI**: Framework multiplataforma para o desenvolvimento do aplicativo.
- **OpenWeather API**: API utilizada para obter os dados climáticos.
- **GPS e Permissões de Localização**: O aplicativo usa o GPS do dispositivo para determinar a localização e fornecer previsões baseadas nela.

## Como Rodar o Projeto

1. Clone este repositório:
   ```bash
   git clone https://github.com/seu-usuario/OpenWeatherMAUI.git
   
2. Abra o projeto no Visual Studio 2022 (com suporte ao .NET MAUI configurado).

3. Configure sua chave da API OpenWeather:
  - Obtenha uma chave de API gratuita em OpenWeather.
  - Substitua <sua-chave-api> no código responsável pelas requisições à API (Services/OpenWeatherService.cs).

4. Conecte um dispositivo ou emulador e execute o aplicativo.

## Requisitos
 - .NET 7.0 ou superior
 - Visual Studio 2022 com o workload .NET MAUI
 - Conta na OpenWeather API para gerar uma chave de API gratuita

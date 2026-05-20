window.masks = {
    // Formata valor monetário sem o prefixo "R$" (o Input Group Bootstrap cuida do label visual)
    money: function (input) {
        let value = input.value.replace(/\D/g, "");
        if (value === "") {
            input.value = "";
            return;
        }
        value = (parseInt(value) / 100).toLocaleString('pt-BR', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
        input.value = value;
        input.dispatchEvent(new Event('change', { bubbles: true }));
    },

    // Máscara de telefone com tolerância a deleção (recebe o evento)
    phone: function (event) {
        const input = event.target;

        // Se o usuário está apagando, deixa o browser agir naturalmente
        const isDeleting = event.inputType === 'deleteContentBackward'
                        || event.inputType === 'deleteContentForward';
        if (isDeleting) return;

        let value = input.value.replace(/\D/g, "");
        if (value.length > 11) value = value.substring(0, 11);

        if (value.length === 0) {
            input.value = "";
            return;
        }

        let formatted = "";
        if (value.length <= 2) {
            formatted = "(" + value;
        } else if (value.length <= 7) {
            formatted = "(" + value.substring(0, 2) + ") " + value.substring(2);
        } else {
            formatted = "(" + value.substring(0, 2) + ") " + value.substring(2, 7) + " - " + value.substring(7);
        }
        input.value = formatted;
        input.dispatchEvent(new Event('change', { bubbles: true }));
    },

    // Formata peso em gramas com 2 casas decimais (sem " g" — o Input Group cuida disso)
    weight: function (input) {
        let value = input.value.replace(/\D/g, "");
        if (value === "") {
            input.value = "";
            return;
        }
        input.value = parseInt(value).toLocaleString('pt-BR');
        input.dispatchEvent(new Event('change', { bubbles: true }));
    },

    // Formata peso com 2 casas decimais sem a string " g"
    weightUnit: function (input) {
        let value = input.value.replace(/\D/g, "");
        if (value === "") {
            input.value = "";
            return;
        }
        value = (parseInt(value) / 100).toLocaleString('pt-BR', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
        input.value = value;
        input.dispatchEvent(new Event('change', { bubbles: true }));
    },

    // Formata percentual sem a string " %" (o Input Group cuida do label visual)
    percentage: function (input) {
        let value = input.value.replace(/\D/g, "");
        if (value === "") {
            input.value = "";
            return;
        }
        value = (parseInt(value) / 100).toLocaleString('pt-BR', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
        input.value = value;
        input.dispatchEvent(new Event('change', { bubbles: true }));
    },

    // Máscara de tempo HH:MM com tolerância a deleção (recebe o evento)
    time: function (event) {
        const input = event.target;

        // Se o usuário está apagando, deixa o browser agir naturalmente
        const isDeleting = event.inputType === 'deleteContentBackward'
                        || event.inputType === 'deleteContentForward';
        if (isDeleting) return;

        let value = input.value.replace(/\D/g, "");
        if (value.length > 4) value = value.substring(0, 4);
        if (value === "") {
            input.value = "";
            return;
        }
        if (value.length > 2) {
            input.value = value.substring(0, 2) + ":" + value.substring(2);
        } else {
            input.value = value;
        }
        input.dispatchEvent(new Event('change', { bubbles: true }));
    },

    // Lê o valor de um elemento passado via ElementReference do Blazor
    getInputValue: function (element) {
        return element ? element.value : "";
    },

    // Define o valor de um elemento passado via ElementReference do Blazor
    setInputValue: function (element, value) {
        if (element) {
            element.value = value;
        }
    }
};

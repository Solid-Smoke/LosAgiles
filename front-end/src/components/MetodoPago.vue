<template>
    <MainNavbar />
    <div class="container mt-5">
      <h2 class="text-center">Seleccione su forma de pago</h2>

      <div class="payment-options">
        <div class="payment-option" :class="{ selected: paymentMethod === 'sinpe' }" @click="paymentMethod = 'sinpe'">
          <div class="icon">
            <img :src="require('@/assets/SINPE.png')">
          </div>
          <h5>SINPE</h5>
          <p>Seleccione esta opción para pagar mediante SINPE y adjuntar el comprobante.</p>
        </div>

        <div  class="payment-option" :class="{ selected: paymentMethod === 'creditCard' }"  @click="paymentMethod = 'creditCard'">
          <div class="icon">
            <img :src="require('@/assets/CREDITCARD.png')" alt="Credit Card Icon" />
          </div>
          <h5>Tarjeta de Crédito/Débito</h5>
          <p>Seleccione esta opción para pagar con tarjeta de crédito o débito.</p>
        </div>
      </div>

      <div v-if="paymentMethod === 'sinpe'" class="mt-4">
        <label for="sinpeFile" class="form-label"><strong>Comprobante de Pago SINPE</strong></label><br>
        <input type="file" id="sinpeFile" @change="onFileChange" accept=".png" class="form-control-file" required/>
        <p v-if="sinpeReceiptName" class="mt-2">Archivo seleccionado: {{ sinpeReceiptName }}</p><br>
        <button type="button" class="btn btn-success mt-3" @click="validateSinpePayment">Finalizar Pago</button>
      </div>

      <div v-if="paymentMethod === 'creditCard'" class="mt-4">
        <div class="form-group">
          <label for="cardName" class="form-label"><strong>Nombre en la Tarjeta</strong></label>
          <input type="text" v-model="cardName" class="form-control" id="cardName" placeholder="Nombre tal como aparece en la tarjeta" required/>
        </div>
        
        <div class="form-group">
          <label for="cardNumber" class="form-label"><strong>Número de Tarjeta</strong></label>
          <input type="text" v-model="cardNumber" class="form-control" id="cardNumber" maxlength="16" placeholder="XXXX XXXX XXXX XXXX" required @input="validateCardNumber" />
        </div>
        
        <div class="form-group">
          <label for="cardExpiry" class="form-label"><strong>Fecha de Vencimiento</strong></label>
          <div class="d-flex align-items-center">
            <select v-model="cardExpiryMonth" required class="form-control" style="width: 70px; margin-right: 5px">
              <option v-for="month in months" :key="month.value" :value="month.value">{{ month.text }}</option>
            </select>

            <strong>/</strong>

            <select v-model="cardExpiryYear" required class="form-control" style="width: 70px; margin-left: 5px;">
              <option v-for="year in years" :key="year.value" :value="year.value">{{ year.text }}</option>
            </select>
          </div>
        </div>
        
        <div class="form-group">
          <label for="cardCVV" class="form-label"><strong>Código CVV</strong></label>
          <input type="text" v-model="cardCVV" class="form-control" id="cardCVV" maxlength="3" placeholder="XXX" required @input="validateCVV" />
        </div>
        <button type="button" class="btn btn-success mt-3" @click="validateCreditCardPayment">Finalizar Pago</button>
      </div>

      <b-modal v-model="showSinpeModal" title="Error de Pago SINPE" hide-footer centered>
        <p class="my-2">Por favor, adjunte el comprobante de pago para continuar.</p>
        <b-button variant="danger" @click="showSinpeModal = false">Cerrar</b-button>
      </b-modal>

      <b-modal v-model="showCreditCardModal" title="Error de Pago con Tarjeta" hide-footer centered>
        <p class="my-2">{{ creditCardErrorMessage }}</p>
        <b-button variant="danger" @click="showCreditCardModal = false">Cerrar</b-button>
      </b-modal>

      <b-modal v-model="showSuccessModal" title="Pago Realizado" hide-footer centered>
        <p class="my-2">¡Su pago se ha procesado correctamente!</p>
        <b-button variant="success" @click="showSuccessModal = false">Aceptar</b-button>
      </b-modal>
    </div>
</template>

<script>
import MainNavbar from './MainNavbar.vue';

export default {
  components: {
    MainNavbar,
  },
  data() {
    return {
      paymentMethod: null,
      sinpeReceiptName: '',
      cardName: '',
      cardNumber: '',
      cardExpiryMonth: '',
      cardExpiryYear: '',
      cardCVV: '',
      months: [
        { value: 1, text: '01' },
        { value: 2, text: '02' },
        { value: 3, text: '03' },
        { value: 4, text: '04' },
        { value: 5, text: '05' },
        { value: 6, text: '06' },
        { value: 7, text: '07' },
        { value: 8, text: '08' },
        { value: 9, text: '09' },
        { value: 10, text: '10' },
        { value: 11, text: '11' },
        { value: 12, text: '12' }
      ],
      years: (() => {
        const currentYear = new Date().getFullYear();
        const years = [];
        for (let i = 0; i < 10; i++) {
          const yearValue = currentYear + i;
          years.push({
            value: yearValue,
            text: yearValue.toString().slice(-2)
          });
        }
        return years;
      })(),
      showSinpeModal: false,
      showCreditCardModal: false,
      showSuccessModal: false,
      creditCardErrorMessage: '',
    };
  },
  methods: {
    onFileChange(event) {
      const file = event.target.files[0];
      if (file) {
        this.sinpeReceiptName = file.name;
      }
    },
    validateCardNumber() {
      if (this.cardNumber.length > 16) {
        this.cardNumber = this.cardNumber.slice(0, 16);
      }
    },
    validateCVV() {
      if (this.cardCVV.length > 3) {
        this.cardCVV = this.cardCVV.slice(0, 3);
      }
    },
    validateSinpePayment() {
      if (!this.sinpeReceiptName) {
        this.showSinpeModal = true;
        return;
      }
      this.showSuccessModal = true;
    },
    validateCreditCardPayment() {
      const currentMonth = new Date().getMonth() + 1;
      const currentYear = new Date().getFullYear();

      if (!this.cardName || !this.cardNumber || !this.cardExpiryMonth || !this.cardExpiryYear || !this.cardCVV) {
        this.creditCardErrorMessage = "Por favor, complete todos los campos de la tarjeta para continuar.";
        this.showCreditCardModal = true;
        return;
      }

      if (this.cardNumber.length !== 16) {
        this.creditCardErrorMessage = "El número de tarjeta debe contener exactamente 16 dígitos.";
        this.showCreditCardModal = true;
        return;
      }

      if (this.cardCVV.length !== 3) {
        this.creditCardErrorMessage = "El CVV debe contener exactamente 3 dígitos.";
        this.showCreditCardModal = true;
        return;
      }

      if (this.cardExpiryYear < currentYear || (this.cardExpiryYear == currentYear && this.cardExpiryMonth < currentMonth)) {
        this.creditCardErrorMessage = "La fecha de vencimiento debe ser en el futuro.";
        this.showCreditCardModal = true;
        return;
      }

      this.showSuccessModal = true;
    },
  },
};
</script>

<style scoped>
.container {
  max-width: 600px;
}

.payment-options {
  display: flex;
  justify-content: space-between;
  margin-top: 20px;
}

.payment-option {
  cursor: pointer;
  border: 1px solid #ddd;
  border-radius: 8px;
  width: 48%;
  text-align: center;
  padding: 20px;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
}

.payment-option.selected {
  border-color: #007bff;
  background-color: #f0f8ff;
}

.payment-option:hover {
  transform: translateY(-3px);
  box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.2);
}

.payment-option h5 {
  font-size: 1.2rem;
  color: #007bff;
}

.payment-option p {
  font-size: 0.9rem;
  color: #555;
}

.icon img {
  width: 40px;
  height: 40px;
}

.form-label {
  margin-top: 15px;
}
</style>

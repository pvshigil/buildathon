<template>
  <div class="h-full bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden flex flex-col">
    <!-- Header with Intent Tabs and Progress -->
    <div class="px-5 py-4 border-b border-slate-100 bg-slate-50/70 flex flex-col sm:flex-row sm:items-center justify-between gap-3">
      <div class="flex items-center space-x-2.5">
        <div class="w-8 h-8 rounded-lg bg-emerald-600 text-white flex items-center justify-center shadow-sm">
          <svg class="w-4 h-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
            <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
          </svg>
        </div>
        <div>
          <div class="flex items-center space-x-2">
            <h2 class="text-sm font-bold text-slate-800">Transfer Summary & Order Form</h2>
            <span class="text-[10px] font-semibold px-2 py-0.5 rounded-full bg-emerald-100 text-emerald-800">
              Interactive & Editable
            </span>
          </div>
          <p class="text-xs text-slate-500">Auto-extracted from conversation • Edit any field below</p>
        </div>
      </div>

      <!-- Quick Fill Demo Data Button -->
      <button 
        @click="fillDemoData" 
        type="button"
        class="inline-flex items-center justify-center px-2.5 py-1 text-xs font-semibold rounded-lg bg-sky-50 text-sky-700 hover:bg-sky-100 border border-sky-200 transition active:scale-95 shadow-2xs whitespace-nowrap self-start sm:self-auto"
        title="Fill remaining mandatory fields with sample customer data for presentation"
      >
        <span class="mr-1">⚡</span> Quick-Fill Identity
      </button>
    </div>

    <!-- Intent Selector Tabs & Live Completion Bar -->
    <div class="px-5 py-3 bg-white border-b border-slate-100">
      <div class="flex items-center justify-between flex-wrap gap-2 mb-2">
        <!-- Intent Buttons -->
        <div class="inline-flex p-1 bg-slate-100 rounded-xl space-x-1 text-xs font-semibold">
          <button 
            @click="setIntent('Move')" 
            :class="[formData.actionIntent === 'Move' ? 'bg-white text-sky-700 shadow-2xs' : 'text-slate-600 hover:text-slate-900']"
            class="px-3 py-1 rounded-lg transition"
          >
            🚚 Move Service
          </button>
          <button 
            @click="setIntent('Stop')" 
            :class="[formData.actionIntent === 'Stop' ? 'bg-white text-rose-700 shadow-2xs' : 'text-slate-600 hover:text-slate-900']"
            class="px-3 py-1 rounded-lg transition"
          >
            🛑 Stop Service
          </button>
          <button 
            @click="setIntent('Start')" 
            :class="[formData.actionIntent === 'Start' ? 'bg-white text-emerald-700 shadow-2xs' : 'text-slate-600 hover:text-slate-900']"
            class="px-3 py-1 rounded-lg transition"
          >
            ⚡ Start Service
          </button>
        </div>

        <!-- Completion Indicator Badge -->
        <div class="flex items-center space-x-1.5 text-xs font-semibold">
          <span 
            :class="[missingList.length === 0 ? 'bg-emerald-100 text-emerald-800 border-emerald-200' : 'bg-amber-100 text-amber-800 border-amber-200']"
            class="px-2.5 py-0.5 rounded-full border flex items-center space-x-1"
          >
            <span v-if="missingList.length === 0">✓ All Mandatory Fields Verified</span>
            <span v-else>⚠️ {{ missingList.length }} Required Field{{ missingList.length > 1 ? 's' : '' }} Missing</span>
          </span>
        </div>
      </div>

      <!-- Progress Meter -->
      <div class="w-full bg-slate-100 h-1.5 rounded-full overflow-hidden">
        <div 
          class="h-full transition-all duration-300 rounded-full"
          :class="[missingList.length === 0 ? 'bg-emerald-500' : 'bg-sky-500']"
          :style="{ width: `${completionPercentage}%` }"
        ></div>
      </div>
    </div>

    <!-- Body Form: Editable Fields -->
    <div class="flex-1 overflow-y-auto p-5 space-y-5">
      <!-- SECTION 1: ACCOUNT & IDENTITY VERIFICATION -->
      <div class="bg-slate-50/70 rounded-2xl p-4 border border-slate-200/80 space-y-3.5">
        <div class="flex items-center justify-between pb-1 border-b border-slate-200/60">
          <div class="flex items-center space-x-2">
            <span class="text-xs font-bold uppercase tracking-wider text-slate-700">1. Account & Identity Verification</span>
            <span class="text-[10px] text-slate-400 font-medium">Mandatory for customer authorization</span>
          </div>
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
          <!-- Account Number (Mandatory for Stop & Move) -->
          <div class="sm:col-span-2">
            <label class="flex items-center justify-between text-xs font-semibold text-slate-700 mb-1">
              <span>Account Number</span>
              <span v-if="formData.actionIntent !== 'Start'" class="text-[10px] text-rose-600 font-bold uppercase tracking-wider bg-rose-50 px-1.5 py-0.2 rounded border border-rose-200">
                Required for {{ formData.actionIntent }}
              </span>
              <span v-else class="text-[10px] text-slate-400 font-normal">Optional (New Account)</span>
            </label>
            <div class="relative">
              <input
                v-model="formData.accountNumber"
                @input="onFieldChange"
                type="text"
                placeholder="e.g. ACT-849201"
                class="w-full px-3 py-2 text-xs rounded-xl border font-mono transition focus:outline-none focus:ring-2 focus:ring-sky-500"
                :class="[!formData.accountNumber && formData.actionIntent !== 'Start' ? 'border-amber-300 bg-amber-50/30' : 'border-slate-300 bg-white']"
              />
            </div>
          </div>

          <!-- First Name -->
          <div>
            <label class="flex items-center justify-between text-xs font-semibold text-slate-700 mb-1">
              <span>First Name</span>
              <span class="text-[10px] text-rose-600 font-bold">Required</span>
            </label>
            <input
              v-model="formData.firstName"
              @input="onFieldChange"
              type="text"
              placeholder="e.g. John"
              class="w-full px-3 py-2 text-xs rounded-xl border transition focus:outline-none focus:ring-2 focus:ring-sky-500"
              :class="[!formData.firstName ? 'border-amber-300 bg-amber-50/30' : 'border-slate-300 bg-white']"
            />
          </div>

          <!-- Last Name -->
          <div>
            <label class="flex items-center justify-between text-xs font-semibold text-slate-700 mb-1">
              <span>Last Name</span>
              <span class="text-[10px] text-rose-600 font-bold">Required</span>
            </label>
            <input
              v-model="formData.lastName"
              @input="onFieldChange"
              type="text"
              placeholder="e.g. Doe"
              class="w-full px-3 py-2 text-xs rounded-xl border transition focus:outline-none focus:ring-2 focus:ring-sky-500"
              :class="[!formData.lastName ? 'border-amber-300 bg-amber-50/30' : 'border-slate-300 bg-white']"
            />
          </div>

          <!-- SSN -->
          <div>
            <label class="flex items-center justify-between text-xs font-semibold text-slate-700 mb-1">
              <span>SSN (Social Security #)</span>
              <span class="text-[10px] text-rose-600 font-bold">Required</span>
            </label>
            <div class="relative">
              <input
                v-model="formData.ssn"
                @input="onFieldChange"
                :type="showSsn ? 'text' : 'password'"
                placeholder="XXX-XX-XXXX"
                class="w-full px-3 py-2 text-xs rounded-xl border font-mono transition focus:outline-none focus:ring-2 focus:ring-sky-500 pr-14"
                :class="[!formData.ssn ? 'border-amber-300 bg-amber-50/30' : 'border-slate-300 bg-white']"
              />
              <button 
                type="button" 
                @click="showSsn = !showSsn"
                class="absolute right-2 top-2 text-[10px] font-semibold text-slate-500 hover:text-slate-800 px-1.5 py-0.5 rounded bg-slate-100 border border-slate-200"
              >
                {{ showSsn ? 'Hide' : 'Show' }}
              </button>
            </div>
          </div>

          <!-- DOB -->
          <div>
            <label class="flex items-center justify-between text-xs font-semibold text-slate-700 mb-1">
              <span>Date of Birth (DOB)</span>
              <span class="text-[10px] text-rose-600 font-bold">Required</span>
            </label>
            <input
              v-model="formData.dob"
              @input="onFieldChange"
              type="date"
              class="w-full px-3 py-2 text-xs rounded-xl border transition focus:outline-none focus:ring-2 focus:ring-sky-500"
              :class="[!formData.dob ? 'border-amber-300 bg-amber-50/30' : 'border-slate-300 bg-white']"
            />
          </div>
        </div>
      </div>

      <!-- SECTION 2: CONTACT DETAILS -->
      <div class="bg-slate-50/70 rounded-2xl p-4 border border-slate-200/80 space-y-3.5">
        <div class="flex items-center justify-between pb-1 border-b border-slate-200/60">
          <span class="text-xs font-bold uppercase tracking-wider text-slate-700">2. Contact Information</span>
          <span class="text-[10px] text-slate-400 font-medium">For move confirmation & status alerts</span>
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
          <!-- Phone Number -->
          <div>
            <label class="flex items-center justify-between text-xs font-semibold text-slate-700 mb-1">
              <span>Phone Number</span>
              <span class="text-[10px] text-rose-600 font-bold">Required</span>
            </label>
            <input
              v-model="formData.phoneNumber"
              @input="onFieldChange"
              type="tel"
              placeholder="(555) 000-0000"
              class="w-full px-3 py-2 text-xs rounded-xl border transition focus:outline-none focus:ring-2 focus:ring-sky-500"
              :class="[!formData.phoneNumber ? 'border-amber-300 bg-amber-50/30' : 'border-slate-300 bg-white']"
            />
          </div>

          <!-- Email Address -->
          <div>
            <label class="flex items-center justify-between text-xs font-semibold text-slate-700 mb-1">
              <span>Email Address</span>
              <span class="text-[10px] text-rose-600 font-bold">Required</span>
            </label>
            <input
              v-model="formData.email"
              @input="onFieldChange"
              type="email"
              placeholder="customer@example.com"
              class="w-full px-3 py-2 text-xs rounded-xl border transition focus:outline-none focus:ring-2 focus:ring-sky-500"
              :class="[!formData.email ? 'border-amber-300 bg-amber-50/30' : 'border-slate-300 bg-white']"
            />
          </div>
        </div>
      </div>

      <!-- SECTION 3: SERVICE LOCATIONS & DATES -->
      <div class="bg-slate-50/70 rounded-2xl p-4 border border-slate-200/80 space-y-3.5">
        <div class="flex items-center justify-between pb-1 border-b border-slate-200/60">
          <span class="text-xs font-bold uppercase tracking-wider text-slate-700">3. Service Locations & Schedule</span>
          <span class="text-[10px] text-slate-400 font-medium">Field technician dispatch details</span>
        </div>

        <div class="space-y-3">
          <!-- Current / Disconnect Location -->
          <div class="p-3 bg-white rounded-xl border border-slate-200 space-y-2">
            <div class="flex items-center justify-between">
              <span class="text-xs font-bold text-slate-800 flex items-center">
                <span class="w-2 h-2 rounded-full bg-rose-500 mr-1.5"></span>
                {{ formData.actionIntent === 'Start' ? 'Service Address (Connect)' : 'Current Address (Disconnect)' }}
              </span>
              <span class="text-[10px] text-rose-600 font-bold">Required</span>
            </div>
            
            <input
              v-model="formData.currentAddress"
              @input="onFieldChange"
              type="text"
              placeholder="e.g. 104 Main St, Springfield"
              class="w-full px-3 py-2 text-xs rounded-xl border transition focus:outline-none focus:ring-2 focus:ring-sky-500"
              :class="[!formData.currentAddress ? 'border-amber-300 bg-amber-50/30' : 'border-slate-300 bg-white']"
            />

            <!-- Disconnect Date (Required for Move & Stop) -->
            <div v-if="formData.actionIntent !== 'Start'" class="pt-1 flex items-center justify-between">
              <label class="text-xs font-medium text-slate-600">Stop Date (When to disconnect):</label>
              <input
                v-model="formData.disconnectDate"
                @input="onFieldChange"
                type="date"
                class="px-2.5 py-1 text-xs rounded-lg border border-slate-300 font-medium focus:outline-none focus:ring-2 focus:ring-sky-500"
                :class="[!formData.disconnectDate ? 'border-amber-300 bg-amber-50/30' : '']"
              />
            </div>
          </div>

          <!-- New / Connect Location (Only for Move) -->
          <div v-if="formData.actionIntent === 'Move'" class="p-3 bg-white rounded-xl border border-emerald-200 space-y-2 ring-1 ring-emerald-500/10">
            <div class="flex items-center justify-between">
              <span class="text-xs font-bold text-slate-800 flex items-center">
                <span class="w-2 h-2 rounded-full bg-emerald-500 mr-1.5"></span>
                New Address (Connect Service)
              </span>
              <span class="text-[10px] text-emerald-700 font-bold">Required for Move</span>
            </div>

            <input
              v-model="formData.newAddress"
              @input="onFieldChange"
              type="text"
              placeholder="e.g. 450 Oak Drive, Columbus"
              class="w-full px-3 py-2 text-xs rounded-xl border transition focus:outline-none focus:ring-2 focus:ring-sky-500"
              :class="[!formData.newAddress ? 'border-amber-300 bg-amber-50/30' : 'border-slate-300 bg-white']"
            />

            <!-- Connect Date -->
            <div class="pt-1 flex items-center justify-between">
              <label class="text-xs font-medium text-slate-600">Start Date (When to connect):</label>
              <input
                v-model="formData.connectDate"
                @input="onFieldChange"
                type="date"
                class="px-2.5 py-1 text-xs rounded-lg border border-slate-300 font-medium focus:outline-none focus:ring-2 focus:ring-sky-500"
                :class="[!formData.connectDate ? 'border-amber-300 bg-amber-50/30' : '']"
              />
            </div>
          </div>

          <!-- Connect Date for Start Service -->
          <div v-if="formData.actionIntent === 'Start'" class="p-3 bg-white rounded-xl border border-emerald-200 flex items-center justify-between">
            <label class="text-xs font-bold text-slate-700">Start Date (When to start):</label>
            <input
              v-model="formData.connectDate"
              @input="onFieldChange"
              type="date"
              class="px-2.5 py-1 text-xs rounded-lg border border-slate-300 font-medium focus:outline-none focus:ring-2 focus:ring-sky-500"
              :class="[!formData.connectDate ? 'border-amber-300 bg-amber-50/30' : '']"
            />
          </div>

          <!-- Final Bill Forwarding Address -->
          <div class="p-3 bg-white rounded-xl border border-slate-200 space-y-1.5">
            <div class="flex items-center justify-between">
              <span class="text-xs font-semibold text-slate-700">Forwarding Address (Final Bill)</span>
              <span class="text-[10px] text-slate-400 font-normal">Optional</span>
            </div>
            <input
              v-model="formData.forwardingAddress"
              @input="onFieldChange"
              type="text"
              placeholder="e.g. 777 Pine Ave (Defaults to new address if empty)"
              class="w-full px-3 py-2 text-xs rounded-xl border border-slate-300 transition focus:outline-none focus:ring-2 focus:ring-sky-500"
            />
          </div>
        </div>
      </div>

      <!-- DUAL-SERVICE OVERLAP BANNER -->
      <div v-if="formData.actionIntent === 'Move' && calculatedOverlap > 0" class="bg-gradient-to-br from-amber-50 to-orange-50 border border-amber-200/90 rounded-2xl p-4 shadow-2xs">
        <div class="flex items-start space-x-3">
          <div class="w-8 h-8 rounded-xl bg-amber-500 text-white flex-shrink-0 flex items-center justify-center shadow-sm">
            <svg class="w-4 h-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <circle cx="12" cy="12" r="10"/>
              <polyline points="12 6 12 12 16 14"/>
            </svg>
          </div>
          <div>
            <div class="flex items-center space-x-2">
              <h4 class="text-xs font-bold text-amber-900">
                Continuous Dual-Service Overlap: {{ calculatedOverlap }} Days
              </h4>
              <span class="text-[10px] uppercase font-bold px-2 py-0.2 bg-amber-200/80 text-amber-900 rounded-full">
                Auto-Calculated
              </span>
            </div>
            <p class="text-[11px] text-amber-800 mt-1 leading-relaxed">
              Both properties will have simultaneous utility service from <strong>{{ formData.connectDate }}</strong> to <strong>{{ formData.disconnectDate }}</strong>. You can paint, clean, and heat without moving day freeze.
            </p>
          </div>
        </div>
      </div>

      <!-- MISSING FIELDS WARNING CARD (Shown when incomplete) -->
      <div v-if="missingList.length > 0" class="p-3.5 bg-amber-50/80 rounded-xl border border-amber-200 space-y-1.5">
        <div class="flex items-center space-x-1.5 text-xs font-bold text-amber-900">
          <svg class="w-4 h-4 text-amber-600" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="m21.73 18-8-14a2 2 0 0 0-3.48 0l-8 14A2 2 0 0 0 4 21h16a2 2 0 0 0 1.73-3Z"/>
            <line x1="12" y1="9" x2="12" y2="13"/>
            <line x1="12" y1="17" x2="12.01" y2="17"/>
          </svg>
          <span>Required to Complete {{ formData.actionIntent }} Order:</span>
        </div>
        <ul class="text-[11px] text-amber-800 list-disc list-inside space-y-0.5 pl-1">
          <li v-for="item in missingList" :key="item">
            <strong>{{ item }}</strong> is required.
          </li>
        </ul>
        <p class="text-[10px] text-amber-700 italic pt-1">
          Tip: You can type these into the chat on the left, or fill them directly into the inputs above, or click "Quick-Fill Identity".
        </p>
      </div>

      <!-- CONFIRM & SCHEDULE BUTTON -->
      <div class="pt-2">
        <button
          @click="submitTransfer"
          :disabled="isScheduling || missingList.length > 0"
          class="w-full py-4 px-6 rounded-xl font-bold text-sm text-white transition-all duration-200 flex items-center justify-center space-x-2 shadow-lg"
          :class="[
            missingList.length === 0 && !isScheduling
              ? 'bg-emerald-600 hover:bg-emerald-700 active:bg-emerald-800 shadow-emerald-600/25 cursor-pointer transform hover:-translate-y-0.5' 
              : 'bg-slate-300 text-slate-500 cursor-not-allowed shadow-none'
          ]"
        >
          <svg v-if="!isScheduling && missingList.length === 0" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/>
            <polyline points="22 4 12 14.01 9 11.01"/>
          </svg>
          <svg v-else-if="isScheduling" class="animate-spin w-5 h-5 text-white" viewBox="0 0 24 24" fill="none">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8H4z"></path>
          </svg>
          <span>
            {{ isScheduling ? 'Scheduling Move with Field Ops...' : (missingList.length === 0 ? 'Confirm & Schedule ' + formData.actionIntent : 'Please Complete Mandatory Fields Above') }}
          </span>
        </button>
        <p class="text-center text-[11px] text-slate-400 mt-2">
          Encrypted utility dispatch • Instant confirmation ID & meter dispatch receipt.
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref, computed, watch } from 'vue'

const props = defineProps({
  transferData: {
    type: Object,
    default: () => ({})
  },
  isScheduling: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['confirm', 'update:transferData'])

const showSsn = ref(false)

// Local editable reactive form
const formData = reactive({
  actionIntent: 'Move',
  accountNumber: '',
  firstName: '',
  lastName: '',
  ssn: '',
  dob: '',
  phoneNumber: '',
  email: '',
  currentAddress: '',
  newAddress: '',
  disconnectDate: '',
  connectDate: '',
  forwardingAddress: '',
  overlapDays: 0,
  confidenceScore: 0.98
})

// Sync incoming props to local formData
watch(() => props.transferData, (newData) => {
  if (newData) {
    if (newData.actionIntent) formData.actionIntent = newData.actionIntent
    if (newData.accountNumber !== undefined) formData.accountNumber = newData.accountNumber || ''
    if (newData.firstName !== undefined) formData.firstName = newData.firstName || ''
    if (newData.lastName !== undefined) formData.lastName = newData.lastName || ''
    if (newData.ssn !== undefined) formData.ssn = newData.ssn || ''
    if (newData.dob !== undefined) formData.dob = normalizeDate(newData.dob)
    if (newData.phoneNumber !== undefined) formData.phoneNumber = newData.phoneNumber || ''
    if (newData.email !== undefined) formData.email = newData.email || ''
    if (newData.currentAddress !== undefined) formData.currentAddress = newData.currentAddress || ''
    if (newData.newAddress !== undefined) formData.newAddress = newData.newAddress || ''
    if (newData.disconnectDate !== undefined) formData.disconnectDate = normalizeDate(newData.disconnectDate)
    if (newData.connectDate !== undefined) formData.connectDate = normalizeDate(newData.connectDate)
    if (newData.forwardingAddress !== undefined) formData.forwardingAddress = newData.forwardingAddress || ''
  }
}, { immediate: true, deep: true })

function normalizeDate(d) {
  if (!d) return ''
  // If formatted like 2026-10-28, return as is
  if (/^\d{4}-\d{2}-\d{2}$/.test(d)) return d
  try {
    const parsed = new Date(d)
    if (!isNaN(parsed.getTime())) {
      return parsed.toISOString().split('T')[0]
    }
  } catch (e) {}
  return d
}

// Intent switcher
function setIntent(intent) {
  formData.actionIntent = intent
  onFieldChange()
}

// Calculate overlap dynamically whenever dates change
const calculatedOverlap = computed(() => {
  if (formData.actionIntent !== 'Move') return 0
  if (!formData.connectDate || !formData.disconnectDate) return 0

  const start = new Date(formData.connectDate)
  const end = new Date(formData.disconnectDate)
  if (!isNaN(start.getTime()) && !isNaN(end.getTime()) && start <= end) {
    return Math.floor((end - start) / (1000 * 60 * 60 * 24)) + 1
  }
  return 0
})

// Compute dynamic list of missing fields based on user's exact specification:
// 1. First name, Last name, SSN, DOB, Phone number, Email address: ALL intents
// 2. Account number: Move & Stop
// 3. Current address: ALL intents
// 4. New address: Move only
// 5. DisconnectDate (Stop Date): Move & Stop
// 6. ConnectDate (Start Date): Move & Start
const missingList = computed(() => {
  const missing = []

  // Customer identity
  if (!formData.firstName?.trim()) missing.push('First Name')
  if (!formData.lastName?.trim()) missing.push('Last Name')
  if (!formData.ssn?.trim()) missing.push('SSN')
  if (!formData.dob?.trim()) missing.push('Date of Birth (DOB)')
  if (!formData.phoneNumber?.trim()) missing.push('Phone Number')
  if (!formData.email?.trim()) missing.push('Email Address')

  // Intent-specific rules
  if (formData.actionIntent === 'Move') {
    if (!formData.accountNumber?.trim()) missing.push('Account Number')
    if (!formData.currentAddress?.trim()) missing.push('Current Address (Move-Out)')
    if (!formData.disconnectDate?.trim()) missing.push('Stop Date')
    if (!formData.newAddress?.trim()) missing.push('New Address (Move-In)')
    if (!formData.connectDate?.trim()) missing.push('Start Date')
  } else if (formData.actionIntent === 'Stop') {
    if (!formData.accountNumber?.trim()) missing.push('Account Number')
    if (!formData.currentAddress?.trim()) missing.push('Current Service Address')
    if (!formData.disconnectDate?.trim()) missing.push('Stop Date')
  } else if (formData.actionIntent === 'Start') {
    if (!formData.currentAddress?.trim()) missing.push('Service Address')
    if (!formData.connectDate?.trim()) missing.push('Start Date')
  }

  return missing
})

// Total required fields by intent
const totalRequiredCount = computed(() => {
  if (formData.actionIntent === 'Move') return 11
  if (formData.actionIntent === 'Stop') return 9
  return 8 // Start
})

const completionPercentage = computed(() => {
  const filled = totalRequiredCount.value - missingList.value.length
  return Math.max(5, Math.min(100, Math.round((filled / totalRequiredCount.value) * 100)))
})

function onFieldChange() {
  formData.overlapDays = calculatedOverlap.value
  emit('update:transferData', { ...formData, isComplete: missingList.value.length === 0, missingFields: missingList.value })
}

function fillDemoData() {
  if (!formData.accountNumber) formData.accountNumber = 'ACT-8492019'
  if (!formData.firstName) formData.firstName = 'Sarah'
  if (!formData.lastName) formData.lastName = 'Connor'
  if (!formData.ssn) formData.ssn = '123-45-6789'
  if (!formData.dob) formData.dob = '1985-06-15'
  if (!formData.phoneNumber) formData.phoneNumber = '(555) 234-5678'
  if (!formData.email) formData.email = 'sarah.connor@example.com'
  if (!formData.currentAddress) formData.currentAddress = '104 Main St'
  if (formData.actionIntent === 'Move' && !formData.newAddress) formData.newAddress = '450 Oak Drive'
  if (!formData.disconnectDate && formData.actionIntent !== 'Start') formData.disconnectDate = '2026-10-28'
  if (!formData.connectDate && formData.actionIntent !== 'Stop') formData.connectDate = '2026-10-25'
  if (!formData.forwardingAddress) formData.forwardingAddress = '777 Pine Ave'
  onFieldChange()
}

function submitTransfer() {
  if (missingList.value.length > 0) return
  formData.overlapDays = calculatedOverlap.value
  emit('confirm', { ...formData })
}
</script>

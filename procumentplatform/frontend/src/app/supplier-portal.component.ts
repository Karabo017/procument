import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-supplier-portal',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="supplier-portal">
      <!-- Header -->
      <header class="bg-green-900 text-white p-4">
        <div class="max-w-7xl mx-auto flex justify-between items-center">
          <div class="flex items-center space-x-4">
            <img src="/assets/company-logo.png" alt="Company" class="h-8" />
            <h1 class="text-xl font-bold">Supplier Portal</h1>
          </div>
          <div class="flex items-center space-x-4">
            <span class="text-sm">Welcome, ABC Suppliers Ltd - Supplier</span>
            <button class="bg-green-800 hover:bg-green-700 px-3 py-1 rounded text-sm">Logout</button>
          </div>
        </div>
      </header>

      <!-- Navigation -->
      <nav class="bg-green-800">
        <div class="max-w-7xl mx-auto px-4">
          <div class="flex space-x-8">
            <a href="#" class="text-white py-4 px-2 border-b-2 border-green-500 text-sm font-medium">Dashboard</a>
            <a href="#" class="text-green-100 hover:text-white py-4 px-2 text-sm font-medium">Active Bids</a>
            <a href="#" class="text-green-100 hover:text-white py-4 px-2 text-sm font-medium">Opportunities</a>
            <a href="#" class="text-green-100 hover:text-white py-4 px-2 text-sm font-medium">Documents</a>
            <a href="#" class="text-green-100 hover:text-white py-4 px-2 text-sm font-medium">Profile</a>
          </div>
        </div>
      </nav>

      <!-- Dashboard Content -->
      <main class="max-w-7xl mx-auto py-6 px-4">
        <!-- Quick Stats -->
        <div class="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-semibold text-gray-900 mb-2">Active Bids</h3>
            <p class="text-3xl font-bold text-green-600">7</p>
            <p class="text-sm text-gray-600">In progress</p>
          </div>
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-semibold text-gray-900 mb-2">Won Contracts</h3>
            <p class="text-3xl font-bold text-blue-600">12</p>
            <p class="text-sm text-gray-600">This year</p>
          </div>
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-semibold text-gray-900 mb-2">Total Value</h3>
            <p class="text-3xl font-bold text-purple-600">R2.4M</p>
            <p class="text-sm text-gray-600">Contract value</p>
          </div>
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-semibold text-gray-900 mb-2">Success Rate</h3>
            <p class="text-3xl font-bold text-orange-600">68%</p>
            <p class="text-sm text-gray-600">Bid success</p>
          </div>
        </div>

        <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
          <!-- Active Bids -->
          <div class="bg-white rounded-lg shadow-md">
            <div class="p-6 border-b">
              <div class="flex justify-between items-center">
                <h2 class="text-xl font-semibold text-gray-900">Active Bids</h2>
                <button class="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded text-sm font-medium">
                  View All Bids
                </button>
              </div>
            </div>
            <div class="p-6">
              <div class="space-y-4">
                <div *ngFor="let bid of activeBids" class="border border-gray-200 rounded-lg p-4">
                  <div class="flex justify-between items-start mb-2">
                    <div>
                      <h3 class="font-medium text-gray-900">{{ bid.tenderTitle }}</h3>
                      <p class="text-sm text-gray-600">{{ bid.tenderNumber }}</p>
                    </div>
                    <span class="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium"
                          [ngClass]="{
                            'bg-yellow-100 text-yellow-800': bid.status === 'Draft',
                            'bg-blue-100 text-blue-800': bid.status === 'Submitted',
                            'bg-green-100 text-green-800': bid.status === 'Under Review',
                            'bg-red-100 text-red-800': bid.status === 'Rejected'
                          }">
                      {{ bid.status }}
                    </span>
                  </div>
                  <div class="flex justify-between items-center text-xs text-gray-500 mb-3">
                    <span>Bid Amount: R{{ bid.bidAmount | number }}</span>
                    <span>Closes: {{ bid.closingDate }}</span>
                  </div>
                  <div class="flex space-x-2">
                    <button class="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded text-xs">
                      View Details
                    </button>
                    <button *ngIf="bid.status === 'Draft'" 
                            class="bg-green-600 hover:bg-green-700 text-white px-3 py-1 rounded text-xs">
                      Continue Bid
                    </button>
                    <button class="bg-gray-600 hover:bg-gray-700 text-white px-3 py-1 rounded text-xs">
                      Download Docs
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- New Opportunities -->
          <div class="bg-white rounded-lg shadow-md">
            <div class="p-6 border-b">
              <h2 class="text-xl font-semibold text-gray-900">New Opportunities</h2>
            </div>
            <div class="p-6">
              <div class="space-y-4">
                <div *ngFor="let opportunity of newOpportunities" class="border border-gray-200 rounded-lg p-4">
                  <div class="flex justify-between items-start mb-2">
                    <div>
                      <h3 class="font-medium text-gray-900">{{ opportunity.title }}</h3>
                      <p class="text-sm text-gray-600">{{ opportunity.tenderNumber }}</p>
                    </div>
                    <span class="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium bg-green-100 text-green-800">
                      {{ opportunity.category }}
                    </span>
                  </div>
                  <div class="flex justify-between items-center text-xs text-gray-500 mb-3">
                    <span>Est. Value: R{{ opportunity.estimatedValue | number }}</span>
                    <span>Closes: {{ opportunity.closingDate }}</span>
                  </div>
                  <div class="mb-3">
                    <p class="text-sm text-gray-700">{{ opportunity.description | slice:0:100 }}...</p>
                  </div>
                  <div class="flex space-x-2">
                    <button class="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded text-xs">
                      View Tender
                    </button>
                    <button class="bg-green-600 hover:bg-green-700 text-white px-3 py-1 rounded text-xs">
                      Start Bid
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Recent Activity -->
        <div class="mt-8 bg-white rounded-lg shadow-md">
          <div class="p-6 border-b">
            <h2 class="text-xl font-semibold text-gray-900">Recent Activity</h2>
          </div>
          <div class="p-6">
            <div class="space-y-4">
              <div *ngFor="let activity of recentActivity" class="flex items-start space-x-3">
                <div class="flex-shrink-0">
                  <div class="w-8 h-8 rounded-full flex items-center justify-center text-xs font-medium"
                       [ngClass]="{
                         'bg-blue-100 text-blue-800': activity.type === 'bid',
                         'bg-green-100 text-green-800': activity.type === 'award',
                         'bg-yellow-100 text-yellow-800': activity.type === 'update',
                         'bg-red-100 text-red-800': activity.type === 'rejection'
                       }">
                    {{ activity.icon }}
                  </div>
                </div>
                <div class="flex-1 min-w-0">
                  <p class="text-sm text-gray-900">{{ activity.description }}</p>
                  <p class="text-xs text-gray-500">{{ activity.timestamp }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  `,
  styles: [`
    .supplier-portal {
      min-height: 100vh;
      background-color: #f9fafb;
    }
  `]
})
export class SupplierPortalComponent {
  activeBids = [
    {
      tenderTitle: 'Office Equipment Supply Contract',
      tenderNumber: 'PROC-2025-001',
      status: 'Submitted',
      bidAmount: 450000,
      closingDate: '2025-01-30'
    },
    {
      tenderTitle: 'IT Infrastructure Services',
      tenderNumber: 'PROC-2025-002',
      status: 'Draft',
      bidAmount: 750000,
      closingDate: '2025-02-15'
    },
    {
      tenderTitle: 'Marketing Services Contract',
      tenderNumber: 'PROC-2025-003',
      status: 'Under Review',
      bidAmount: 280000,
      closingDate: '2025-01-25'
    }
  ];

  newOpportunities = [
    {
      title: 'Corporate Catering Services',
      tenderNumber: 'PROC-2025-008',
      category: 'Professional Services',
      estimatedValue: 1200000,
      closingDate: '2025-02-20',
      description: 'Comprehensive catering services for corporate facilities including daily meals, events, and executive dining.'
    },
    {
      title: 'Fleet Management Services',
      tenderNumber: 'PROC-2025-009',
      category: 'Services',
      estimatedValue: 800000,
      closingDate: '2025-02-25',
      description: 'Complete fleet management including maintenance, fuel management, and vehicle tracking systems.'
    }
  ];

  recentActivity = [
    {
      type: 'award',
      icon: '🏆',
      description: 'Congratulations! You won the "Security Services Contract" tender.',
      timestamp: '2 hours ago'
    },
    {
      type: 'bid',
      icon: '📝',
      description: 'Bid submitted for "Office Equipment Supply Contract"',
      timestamp: '1 day ago'
    },
    {
      type: 'update',
      icon: '📄',
      description: 'New documents added to "IT Infrastructure Services" tender',
      timestamp: '2 days ago'
    },
    {
      type: 'rejection',
      icon: '❌',
      description: 'Bid rejected for "Cleaning Services Contract" - feedback available',
      timestamp: '3 days ago'
    }
  ];
}
